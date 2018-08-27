using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ANDP.Lib.Domain.Factories;
using ANDP.Lib.Domain.Models;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;
using Topshelf;
using Timer = System.Timers.Timer;

namespace ANDP.Lib.Domain.Services
{
    public class DispatcherService : ServiceControl
    {
        private readonly object _syncProvisioningStartRoot = new object();
        private Timer _provisioningStartTimer = null;
        private readonly string _user = "ANDP Dispatcher";
        private readonly int _companyId;
        private readonly Guid _tenantId;
        private int _currentThreads = 0;
        private readonly ILogger _iLogger;
        private string _name;

        public DispatcherService(Guid tenantId, int companyId, ILogger iLogger)
        {
            _companyId = companyId;
            _tenantId = tenantId;
            _iLogger = iLogger;
            _name = Assembly.GetExecutingAssembly().GetName().Name + " " +
                    Assembly.GetExecutingAssembly().GetName().Version;
        }

        public bool Start(HostControl hostControl)
        {
            try
            {
                var iEngineService = EngineServiceFactory.Create(_tenantId);
                //Todo: implement this 
                //var iOrderService = OrderServiceFactory.Create(_tenantId);
                //_currentThreads = iOrderService.RetrieveProcessingOrderCount();

                //Get settings for engine for order and service.
                var engineSettings = iEngineService.RetrieveProvisioningEngineSetting(_companyId);
                if (engineSettings == null)
                    throw new Exception("Unable to Retrieve Provisioning Engine Settings by Company Id: " + _companyId);

                //Since this is in seconds we need to multiply by 1000 per second.
                var timeInterval = engineSettings.ProvisioningInterval * 1000;

                // CreateOrder the delegate that invokes methods for the timer.
                //var provisioningStartTimerDelegate = new TimerCallback(OnProvisioningStartElapsedTime);
                //_provisioningStartTimer = new Timer(provisioningStartTimerDelegate, engineSettings, timeInterval, timeInterval);


                _provisioningStartTimer = new Timer(timeInterval);
                _provisioningStartTimer.Elapsed += (sender, e) => OnProvisioningStartElapsedTime(sender, e, engineSettings);
                //This makes sure the timer only executes once until you call start again.
                _provisioningStartTimer.AutoReset = false;
                _provisioningStartTimer.Start();
            }
            catch (Exception ex)
            {
                _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " error while attempting to start timer.", LogLevelType.Error, ex);
            }

            _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " has started.", LogLevelType.Info);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            if (_provisioningStartTimer != null)
            {
                _provisioningStartTimer.Stop();
                _provisioningStartTimer.Dispose();
            }

            _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " has stopped.", LogLevelType.Info);
            return true;
        }

        public void OnProvisioningStartElapsedTime(object sender, ElapsedEventArgs elapsedEventArgs, EngineSetting engineSetting)
        {
            try
            {
                _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " OnProvisioningStartElapsedTime timer elapsed.", LogLevelType.Info);

                if (engineSetting.Schedules != null && engineSetting.Schedules.Any())
                {
                    var now = DateTime.Now;
                    
                    foreach (var schedule in engineSetting.Schedules)
                    {
                        bool scheduleRun = false;

                        switch (now.DayOfWeek)
                        {
                            case DayOfWeek.Sunday:
                                if (schedule.Sunday && schedule.SundayStartTime < now.TimeOfDay && schedule.SundayEndtime > now.TimeOfDay)
                                    scheduleRun = true;

                                break;
                            case DayOfWeek.Monday:
                                if (schedule.Monday && schedule.MondayStartTime < now.TimeOfDay && schedule.MondayEndtime > now.TimeOfDay)
                                    scheduleRun = true;

                                break;
                            case DayOfWeek.Tuesday:
                                if (schedule.Tuesday && schedule.TuesdayStartTime < now.TimeOfDay && schedule.TuesdayEndtime > now.TimeOfDay)
                                    scheduleRun = true;

                                break;
                            case DayOfWeek.Wednesday:
                                if (schedule.Wednesday && schedule.WednesdayStartTime < now.TimeOfDay && schedule.WednesdayEndtime > now.TimeOfDay)
                                    scheduleRun = true;

                                break;
                            case DayOfWeek.Thursday:
                                if (schedule.Thursday && schedule.ThursdayStartTime < now.TimeOfDay && schedule.ThursdayEndtime > now.TimeOfDay)
                                    scheduleRun = true;

                                break;
                            case DayOfWeek.Friday:
                                if (schedule.Friday && schedule.FridayStartTime < now.TimeOfDay && schedule.FridayEndtime > now.TimeOfDay)
                                    scheduleRun = true;

                                break;
                            case DayOfWeek.Saturday:
                                if (schedule.Saturday && schedule.SaturdayStartTime < now.TimeOfDay && schedule.SaturdayEndtime > now.TimeOfDay)
                                    scheduleRun = true;

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (scheduleRun)
                        {
                            Provision();
                            break;
                        }
                    }
                }
                else
                {
                    Provision();
                }

                _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " OnProvisioningStartElapsedTime timer elapsed Finished.", LogLevelType.Info);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application Error", _name + "  Crashed again..." + ex, EventLogEntryType.Error);
                //Some how this is getting hit...
            }

            _provisioningStartTimer.Start();
        }

        private void Provision()
        {
            try
            {
                var iEngineService = EngineServiceFactory.Create(_tenantId);
                var iOrderService = OrderServiceFactory.Create(_tenantId);

                // Get Settings for current companyId
                var engineSettings = iEngineService.RetrieveProvisioningEngineSetting(_companyId);

                //Todo check if this can run right now based off schedule.

                if (engineSettings.ProvisioningPaused)
                {
                    _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " Provisioning Paused.", LogLevelType.Info);
                }
                else
                {
                    //Make this thread safe.
                    //Only allow one timer instance to get in at a time.
                    lock (_syncProvisioningStartRoot)
                    {
                        switch (engineSettings.ProvisionByMethod)
                        {
                            case ProvisionByMethodType.Order:
                                var domainOrder = iOrderService.RetrieveNextOrderToProvisionByCompanyIdAndActionType(_companyId, engineSettings.ProvisionableOrderOrServiceActionTypes);

                                int previousOrderId = 0;
                                //var services = new List<Service>(); 


                                //While there are orders pending and I am not using max threads get another order.
                                while (domainOrder != null && engineSettings.MaxThreadsPerDispatcher > _currentThreads)
                                {
                                    if (previousOrderId != domainOrder.Id)
                                        Task.Factory.StartNew(() => ProcessOrderThread(domainOrder));

                                    //Set current to old.
                                    previousOrderId = domainOrder.Id;

                                    //Get next Service Order
                                    domainOrder = iOrderService.RetrieveNextOrderToProvisionByCompanyIdAndActionType(_companyId, engineSettings.ProvisionableOrderOrServiceActionTypes);
                                }

                                break;
                            case ProvisionByMethodType.Service:
                                var domainService = iOrderService.RetrieveNextServiceToProvisionByCompanyIdAndActionType(_companyId, engineSettings.ProvisionableOrderOrServiceActionTypes);

                                int previousServiceId = 0;

                                //While there are orders pending and I am not using max threads get another order.
                                while (domainService != null && engineSettings.MaxThreadsPerDispatcher > _currentThreads)
                                {
                                    if (previousServiceId != domainService.Id)
                                        Task.Factory.StartNew(() => ProcessServiceThread(domainService));

                                    //Set current to old.
                                    previousServiceId = domainService.Id;

                                    //Get next Service
                                    domainService = iOrderService.RetrieveNextServiceToProvisionByCompanyIdAndActionType(_companyId, engineSettings.ProvisionableOrderOrServiceActionTypes);
                                }

                                break;
                            case ProvisionByMethodType.Item:
                                int orderId = 0;
                                int serviceId = 0;
                                int itemId = 0;
                                int previousItemId = 0;

                                object domainItem = iOrderService.RetrieveNextItemToProvisionByCompanyIdAndActionTypeAndItemType(_companyId, engineSettings.ProvisionableItemActionTypes);

                                if (domainItem is PhoneItem)
                                {
                                    var phoneItem = domainItem as PhoneItem;
                                    itemId = phoneItem.Id;
                                    serviceId = phoneItem.ServiceId;
                                    var service = iOrderService.RetrieveServiceById(serviceId);
                                    orderId = service.OrderId;
                                }

                                if (domainItem is VideoItem)
                                {
                                    var videoItem = domainItem as VideoItem;
                                    itemId = videoItem.Id;
                                    serviceId = videoItem.ServiceId;
                                    var service = iOrderService.RetrieveServiceById(serviceId);
                                    orderId = service.OrderId;
                                }

                                if (domainItem is InternetItem)
                                {
                                    var internetItem = domainItem as InternetItem;
                                    itemId = internetItem.Id;
                                    serviceId = internetItem.ServiceId;
                                    var service = iOrderService.RetrieveServiceById(serviceId);
                                    orderId = service.OrderId;
                                }

                                previousItemId = 0;

                                //While there are orders pending and I am not using max threads get another order.
                                while (domainItem != null && engineSettings.MaxThreadsPerDispatcher > _currentThreads)
                                {
                                    if (previousItemId != itemId)
                                        Task.Factory.StartNew(() => ProcessItemThread(orderId, serviceId, itemId));

                                    //Set current to old.
                                    previousItemId = itemId;

                                    //Get next Service
                                    domainItem = iOrderService.RetrieveNextItemToProvisionByCompanyIdAndActionTypeAndItemType(_companyId, engineSettings.ProvisionableItemActionTypes);
                                }

                                break;
                            default:
                                throw new NotImplementedException(_name + " this type is not implemented yet.");
                        }

                        // Continue on this thread... 
                    }
                }
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " error on retrieving next order.", LogLevelType.Error, ex);
                }
            }
            catch (Exception ex)
            {
                _iLogger.WriteLogEntry(_tenantId.ToString(), null, _name + " error on retrieving next order.", LogLevelType.Error, ex);
            }
        }

        private void ProcessOrderThread(Order order)
        {
            try
            {
                Interlocked.Increment(ref _currentThreads);

                var provisioningEngineService = ProvisioningEngineServiceFactory.Create(_tenantId);
                Task.Run(() => provisioningEngineService.ProvisionOrder(order.Id, _companyId, false, true, false, _user));
            }
            catch (Exception ex)
            {
                Exception tempException = ex;
                while (tempException.InnerException != null)
                {
                    tempException = tempException.InnerException;
                }

                string entityValidationErrors = "";
                if (ex is DbEntityValidationException)
                {
                    foreach (var errors in ((DbEntityValidationException) ex).EntityValidationErrors)
                    {
                        foreach (var error in errors.ValidationErrors)
                        {
                            entityValidationErrors += "In Entity " + errors.Entry.Entity.GetType().Name + " - " + error.ErrorMessage;
                        }
                    }
                }

                var exceptionData = new List<object>
                {
                    entityValidationErrors
                };

                _iLogger.WriteLogEntry(_tenantId.ToString(), exceptionData, _name + " error on ProcessOrderThread().", LogLevelType.Error, tempException);
            }
            finally
            {
                Interlocked.Decrement(ref _currentThreads);
            }
        }

        private void ProcessServiceThread(Service service)
        {
            try
            {
                Interlocked.Increment(ref _currentThreads);

                var provisioningEngineService = ProvisioningEngineServiceFactory.Create(_tenantId);
                Task.Run(() => provisioningEngineService.ProvisionService(service.OrderId, service.Id, _companyId, false, true, false, _user));
            }
            catch (Exception ex)
            {
                Exception tempException = ex;
                while (tempException.InnerException != null)
                {
                    tempException = tempException.InnerException;
                }

                string entityValidationErrors = "";
                if (ex is DbEntityValidationException)
                {
                    foreach (var errors in ((DbEntityValidationException) ex).EntityValidationErrors)
                    {
                        foreach (var error in errors.ValidationErrors)
                        {
                            entityValidationErrors += "In Entity " + errors.Entry.Entity.GetType().Name + " - " + error.ErrorMessage;
                        }
                    }
                }

                var exceptionData = new List<object>
                {
                    entityValidationErrors
                };

                _iLogger.WriteLogEntry(_tenantId.ToString(), exceptionData, _name + " error on ProcessServiceThread().", LogLevelType.Error, tempException);
            }
            finally
            {
                Interlocked.Decrement(ref _currentThreads);
            }
        }


        private void ProcessItemThread(int orderId, int serviceId, int itemId)
        {
            try
            {
                Interlocked.Increment(ref _currentThreads);
                var provisioningEngineService = ProvisioningEngineServiceFactory.Create(_tenantId);
                Task.Run(() => provisioningEngineService.ProvisionItem(orderId, serviceId, itemId, _companyId, false, true, false, _user));
            }
            catch (Exception ex)
            {
                Exception tempException = ex;
                while (tempException.InnerException != null)
                {
                    tempException = tempException.InnerException;
                }

                string entityValidationErrors = "";
                if (ex is DbEntityValidationException)
                {
                    foreach (var errors in ((DbEntityValidationException) ex).EntityValidationErrors)
                    {
                        foreach (var error in errors.ValidationErrors)
                        {
                            entityValidationErrors += "In Entity " + errors.Entry.Entity.GetType().Name + " - " + error.ErrorMessage;
                        }
                    }
                }

                var exceptionData = new List<object>
                {
                    entityValidationErrors
                };

                _iLogger.WriteLogEntry(_tenantId.ToString(), exceptionData, _name + " error on ProcessItemThread().", LogLevelType.Error, tempException);
            }
            finally
            {
                Interlocked.Decrement(ref _currentThreads);
            }
        }
    }
}
