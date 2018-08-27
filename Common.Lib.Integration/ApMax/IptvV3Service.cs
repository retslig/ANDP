using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Common.ApAdmin;
using Common.IPTVServiceV3;
using Common.Lib.Extensions;
using Common.ServiceReport;
using EquipmentConnectionSetting = Common.Lib.Domain.Common.Models.EquipmentConnectionSetting;

namespace Common.ApMax
{
    public class IptvV3Service
    {
        private readonly EquipmentConnectionSetting _setting;
        private readonly ServiceVersions _versions;
        private readonly LoginInformation _loginInformation;
        private readonly IPTVServiceClient _iptvService;

        public IptvV3Service(EquipmentConnectionSetting setting, ServiceVersions serviceVersions)
        {
            _setting = setting;
            _versions = serviceVersions;

            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + _setting.Ip + ":" + _setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));
            
            //http://oasislap7:8731/Design_Time_Addresses/IPTVServiceV3/IPTVService/
            _iptvService = new IPTVServiceClient("WSHttpBinding_IIPTVService", new EndpointAddress("http://" + _setting.Ip + ":" + _setting.Port + "/Design_Time_Addresses/IPTVServiceV3/IPTVService/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public IEnumerable<IPTVAccountType> RetrieveIptvAccountTypesByMac(string macAddress)
        {
            var iptvAccountTypes = _iptvService.FindSubscribersByMAC(_loginInformation.LoginToken, macAddress).ToList();
            iptvAccountTypes = FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);
            return iptvAccountTypes;
        }

        public IEnumerable<IPTVAccountType> RetrieveIptvAccountTypesBySerialNumber(string serialNumber)
        {
            var iptvAccountTypes = _iptvService.FindSubscribersBySerialNumber(_loginInformation.LoginToken, serialNumber).ToList();
            iptvAccountTypes = FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);
            return iptvAccountTypes;
        }

        public IEnumerable<IPTVAccountType> RetrieveIptvAccountTypesByBillingAccountNumber(string billingAccountNumber)
        {
            var iptvAccountTypes = _iptvService.GetSubscriberAccountsByBilling(_loginInformation.LoginToken, billingAccountNumber).ToList();
            //Apmax will return the channel package Id but nothing for the package name. So if this happens then we look up the package name by the package id.
            iptvAccountTypes = FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);
            return iptvAccountTypes;
        }

        public IEnumerable<IPTVAccountType> RetrieveIptvAccountsBySubAddress(string subAddress)
        {
            subAddress = subAddress.PadLeft(10, '0');
            var iptvAccountTypes = _iptvService.GetAccountsBySubAddress(_loginInformation.LoginToken, subAddress).ToList();
            //Apmax will return the channel package Id but nothing for the package name. So if this happens then we look up the package name by the package id.
            iptvAccountTypes = FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);
            return iptvAccountTypes;
        }

        public IPTVAccountType RetrieveIptvAccountByServiceReference(string serviceReference)
        {
            var iptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);
            //Apmax will return the channel package Id but nothing for the package name. So if this happens then we look up the package name by the package id.
            var iptvAccountTypes = FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(new List<IPTVAccountType> { iptvAccountType });

            if (iptvAccountTypes != null && iptvAccountTypes.Any())
            {
                return iptvAccountTypes.FirstOrDefault();
            }

            return null;
        }

        public IPTVAccountType RetrieveIptvAccountBySubAddressAndServiceRef(string subAddress, string serviceReference)
        {
            subAddress = subAddress.PadLeft(10, '0');

            var myIptvAccountTypeArray = _iptvService.GetAccountsBySubAddress(_loginInformation.LoginToken, subAddress).ToList();
            var myCurrentIptvAccountType = new IPTVAccountType();

            foreach (IPTVAccountType myIptvAccountType in myIptvAccountTypeArray)
            {
                if (myIptvAccountType.ServiceReference.Trim().Equals(serviceReference.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    myCurrentIptvAccountType = myIptvAccountType;
                    break;
                }
            }

            //Apmax will return the channel package Id but nothing for the package name. So if this happens then we look up the package name by the package id.
            var myCurrentIptvAccountTypes = FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(new List<IPTVAccountType> { myCurrentIptvAccountType });

            if (myCurrentIptvAccountTypes != null && myCurrentIptvAccountTypes.Any())
            {
                return myCurrentIptvAccountTypes.FirstOrDefault();
            }

            return null;
        }

        public ChannelLineupType[] RetrieveAllChannelLineups()
        {
            var channelLineupTypes = _iptvService.GetAllChannelLineups(_loginInformation.LoginToken);
            return channelLineupTypes;
        }

        public void SetIptvAccount2(IPTVAccountType iptvAccountType, SubscriberType subscriberType)
        {
            ////  <IPTVAccountType>
            ////    <ExtensionData />
            ////    <AccountDescription />
            ////    <Active>true</Active>
            ////    <ChannelPackageList>
            ////      <ChannelPackageType>
            ////        <ExtensionData />
            ////        <PackageID>1b1cb36c-15f8-4970-b5b9-b8f329aee7ad</PackageID>
            ////        <PackageName>Test</PackageName>
            ////      </ChannelPackageType>
            ////    </ChannelPackageList>
            ////    <CurrentAmountCharged>0</CurrentAmountCharged>
            ////    <DeactivateReason />
            ////    <FIPSCountyCode>35</FIPSCountyCode>
            ////    <FIPSStateCode>46</FIPSStateCode>
            ////    <MaxBandwidthKbps>0</MaxBandwidthKbps>
            ////    <MaxChargingLimit>0</MaxChargingLimit>
            ////    <PurchasePIN />
            ////    <RatingPIN />
            ////    <ServiceAreaID>754cd53a-30c7-4130-8fa7-0a4d53f0b1be</ServiceAreaID>
            ////    <ServiceReference>6059974200</ServiceReference>
            ////    <SubscriberID>73cd8543-2c99-44c6-ac87-8702c3174e49</SubscriberID>
            ////    <SubscriberName>CHR Solutions</SubscriberName>
            ////  </IPTVAccountType>

            Guid subscriberGuid = new Guid();

            //May need this if the subscriber already exists.
            string billingServiceAddress = null;
            if (!string.IsNullOrEmpty(subscriberType.BillingServiceAddress))
            {
                billingServiceAddress = subscriberType.BillingServiceAddress;
            }

            if (!string.IsNullOrEmpty(iptvAccountType.SubscriberID))
            {
                var subscriberId = iptvAccountType.SubscriberID;
                if (!string.IsNullOrEmpty(subscriberId))
                {
                    subscriberGuid = Guid.Parse(subscriberId);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(iptvAccountType.ServiceReference))
                {
                    subscriberGuid = Guid.Parse(_iptvService.GetSubscriberAccount(_loginInformation.LoginToken, iptvAccountType.ServiceReference).SubscriberID);
                }
            }

            if (subscriberGuid == Guid.Empty)
            {
                throw new Exception("Unable to retrieve a valid guid from the Service Reference passed in.");
            }

            var packagesInApMax = RetrieveAllChannelLineups();

            foreach (var channelPackageType in iptvAccountType.ChannelPackageList)
            {
                if (String.IsNullOrWhiteSpace(channelPackageType.PackageID))
                {
                    channelPackageType.PackageID = (from p in packagesInApMax
                                                    from cp in p.ChannelPackages
                                                    where String.Equals(cp.PackageName.Trim(), channelPackageType.PackageName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                                    select cp.PackageID).FirstOrDefault();
                }
            }

            //Get the existing subscriber info.
            var subscriberVersion = _versions.Subscriber;

            switch (subscriberVersion)
            {
                case 3:
                    var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                    SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByGuid(subscriberGuid);

                    if (existingSubscriberTypeV3 != null)
                    {
                        subscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                        if (subscriberType == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber existed but couldn't convert to Subscriber.SubscriberType from IPTV.SubscriberType.");
                        }

                        if (subscriberType.SubscriberGuid == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber already exists but couldn't determine GUID. SubscriberType:" + subscriberType.SerializeObjectToJsonString());
                        }

                        //Always set the guid on the account if the subscriber already exists.
                        iptvAccountType.SubscriberID = subscriberType.SubscriberGuid;

                        //Always set the Billing Service Address on the account if the subscriber already exists.
                        if (!string.IsNullOrEmpty(billingServiceAddress))
                        {
                            subscriberType.BillingServiceAddress = billingServiceAddress;
                        }
                    }
                    break;
                case 4:
                    var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                    SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByGuid(subscriberGuid);

                    if (existingSubscriberTypeV4 != null)
                    {
                        subscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                        if (subscriberType == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber existed but couldn't convert to Subscriber.SubscriberType from IPTV.SubscriberType.");
                        }

                        if (subscriberType.SubscriberGuid == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber already exists but couldn't determine GUID. SubscriberType:" + subscriberType.SerializeObjectToJsonString());
                        }

                        //Always set the guid on the account if the subscriber already exists.
                        iptvAccountType.SubscriberID = subscriberType.SubscriberGuid;

                        //Always set the Billing Service Address on the account if the subscriber already exists.
                        if (!string.IsNullOrEmpty(billingServiceAddress))
                        {
                            subscriberType.BillingServiceAddress = billingServiceAddress;
                        }

                        //throw new Exception("got here and subscriber exists" + Environment.NewLine + iptvAccountType.SerializeObjectToJsonString() + Environment.NewLine + subscriberType.SerializeObjectToJsonString());
                    }

                    //throw new Exception("got here and subscriber didn't exist" + Environment.NewLine + iptvAccountType.SerializeObjectToJsonString() + Environment.NewLine + subscriberType.SerializeObjectToJsonString());
                    break;
                default:
                    throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
            }

            _iptvService.SetIPTVAccount(_loginInformation.LoginToken, iptvAccountType, subscriberType);
        }

        public void SetIptvAccount(IPTVAccountType iptvAccountType, SubscriberType subscriberType)
        {
            ////  <IPTVAccountType>
            ////    <ExtensionData />
            ////    <AccountDescription />
            ////    <Active>true</Active>
            ////    <ChannelPackageList>
            ////      <ChannelPackageType>
            ////        <ExtensionData />
            ////        <PackageID>1b1cb36c-15f8-4970-b5b9-b8f329aee7ad</PackageID>
            ////        <PackageName>Test</PackageName>
            ////      </ChannelPackageType>
            ////    </ChannelPackageList>
            ////    <CurrentAmountCharged>0</CurrentAmountCharged>
            ////    <DeactivateReason />
            ////    <FIPSCountyCode>35</FIPSCountyCode>
            ////    <FIPSStateCode>46</FIPSStateCode>
            ////    <MaxBandwidthKbps>0</MaxBandwidthKbps>
            ////    <MaxChargingLimit>0</MaxChargingLimit>
            ////    <PurchasePIN />
            ////    <RatingPIN />
            ////    <ServiceAreaID>754cd53a-30c7-4130-8fa7-0a4d53f0b1be</ServiceAreaID>
            ////    <ServiceReference>6059974200</ServiceReference>
            ////    <SubscriberID>73cd8543-2c99-44c6-ac87-8702c3174e49</SubscriberID>
            ////    <SubscriberName>CHR Solutions</SubscriberName>
            ////  </IPTVAccountType>

            //May need this if the subscriber already exists.
            string billingServiceAddress = null;
            if (!string.IsNullOrEmpty(subscriberType.BillingServiceAddress))
            {
                billingServiceAddress = subscriberType.BillingServiceAddress;
            }

            //This is in case the customer does not have a phone.  They then can use the account number which is only 7 digits but AP requires 10.
            string subscriberDefaultPhoneNumber = subscriberType.SubscriberDefaultPhoneNumber.PadLeft(10, '0');

            var packagesInApMax = RetrieveAllChannelLineups();

            foreach (var channelPackageType in iptvAccountType.ChannelPackageList)
            {
                if (String.IsNullOrWhiteSpace(channelPackageType.PackageID))
                {
                    channelPackageType.PackageID = (from p in packagesInApMax
                                                    from cp in p.ChannelPackages
                                                    where String.Equals(cp.PackageName.Trim(), channelPackageType.PackageName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                                    select cp.PackageID).FirstOrDefault();
                }
            }

            //Get the existing subscriber info.
            var subscriberVersion = _versions.Subscriber;

            switch (subscriberVersion)
            {
                case 3:
                    var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                    SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByDefaultPhoneNumber(subscriberDefaultPhoneNumber);

                    if (existingSubscriberTypeV3 != null)
                    {
                        subscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                        if (subscriberType == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber existed but couldn't convert to Subscriber.SubscriberType from IPTV.SubscriberType.");
                        }

                        if (subscriberType.SubscriberGuid == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber already exists but couldn't determine GUID. SubscriberType:" + subscriberType.SerializeObjectToJsonString());
                        }

                        //Always set the guid on the account if the subscriber already exists.
                        iptvAccountType.SubscriberID = subscriberType.SubscriberGuid;

                        //Always set the Billing Service Address on the account if the subscriber already exists.
                        if (!string.IsNullOrEmpty(billingServiceAddress))
                        {
                            subscriberType.BillingServiceAddress = billingServiceAddress;
                        }
                    }
                    break;
                case 4:
                    var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                    SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByDefaultPhoneNumber(subscriberDefaultPhoneNumber);

                    if (existingSubscriberTypeV4 != null)
                    {
                        subscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                        if (subscriberType == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber existed but couldn't convert to Subscriber.SubscriberType from IPTV.SubscriberType.");
                        }

                        if (subscriberType.SubscriberGuid == null)
                        {
                            throw new Exception("SetIptvAccount: Subscriber already exists but couldn't determine GUID. SubscriberType:" + subscriberType.SerializeObjectToJsonString());
                        }

                        //Always set the guid on the account if the subscriber already exists.
                        iptvAccountType.SubscriberID = subscriberType.SubscriberGuid;

                        //Always set the Billing Service Address on the account if the subscriber already exists.
                        if (!string.IsNullOrEmpty(billingServiceAddress))
                        {
                            subscriberType.BillingServiceAddress = billingServiceAddress;
                        }

                        //throw new Exception("got here and subscriber exists" + Environment.NewLine + iptvAccountType.SerializeObjectToJsonString() + Environment.NewLine + subscriberType.SerializeObjectToJsonString());
                    }

                    //throw new Exception("got here and subscriber didn't exist" + Environment.NewLine + iptvAccountType.SerializeObjectToJsonString() + Environment.NewLine + subscriberType.SerializeObjectToJsonString());
                    break;
                default:
                    throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
            }

            _iptvService.SetIPTVAccount(_loginInformation.LoginToken, iptvAccountType, subscriberType);
        }

        public void SetIptvAccountExistingSubscriber(IPTVAccountType iptvAccountType, string subscriberDefaultPhoneNumber)
        {
            //May need this if the subscriber already exists.
            //string billingServiceAddress = subscriberType.BillingServiceAddress;

            //This is in case the customer does not have a phone.  They then can use the account number which is only 7 digits but AP requires 10.
            //string subscriberDefaultPhoneNumber = subscriberType.SubscriberDefaultPhoneNumber.PadLeft(10, '0');

            var packagesInApMax = RetrieveAllChannelLineups();

            foreach (var channelPackageType in iptvAccountType.ChannelPackageList)
            {
                if (String.IsNullOrWhiteSpace(channelPackageType.PackageID))
                {
                    channelPackageType.PackageID = (from p in packagesInApMax
                                                    from cp in p.ChannelPackages
                                                    where String.Equals(cp.PackageName.Trim(), channelPackageType.PackageName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                                    select cp.PackageID).FirstOrDefault();
                }
            }

            //Get the existing subscriber info.
            var subscriberVersion = _versions.Subscriber;

            switch (subscriberVersion)
            {
                case 3:
                    var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                    SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByDefaultPhoneNumber(subscriberDefaultPhoneNumber);

                    if (existingSubscriberTypeV3 != null)
                    {
                        var subscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                        //Always set the guid on the account if the subscriber already exists.
                        iptvAccountType.SubscriberID = subscriberType.SubscriberGuid;

                        _iptvService.SetIPTVAccount(_loginInformation.LoginToken, iptvAccountType, subscriberType);
                    }
                    break;
                case 4:
                    var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                    SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByDefaultPhoneNumber(subscriberDefaultPhoneNumber);

                    if (existingSubscriberTypeV4 != null)
                    {
                        var subscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                        //Always set the guid on the account if the subscriber already exists.
                        iptvAccountType.SubscriberID = subscriberType.SubscriberGuid;

                        _iptvService.SetIPTVAccount(_loginInformation.LoginToken, iptvAccountType, subscriberType);
                    }
                    break;
                default:
                    throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
            }

        }

        public void UpdateIptvServiceReference(string oldServiceReference, string newServiceReference)
        {
            _iptvService.ChangeServiceReference(_loginInformation.LoginToken, oldServiceReference, newServiceReference);
        }

        public void UpdateIptvChannelPackageList(string serviceReference, List<ChannelPackageType> channelPackageTypes)
        {
            var iptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);

            if (iptvAccountType == null)
                throw new Exception("Error IPTV Account null. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

            if (iptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

            var iptvSubscriberType = RetrieveIptvSubscriberFromSubscriberGuid(Guid.Parse(iptvAccountType.SubscriberID));

            //<?xml version="1.0"?>
            //<ArrayOfChannelPackageType xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            //  <ChannelPackageType>
            //    <PackageID>test</PackageID>
            //    <PackageName>test</PackageName>
            //  </ChannelPackageType>
            //  <ChannelPackageType>
            //    <PackageID>test4</PackageID>
            //    <PackageName>test4</PackageName>
            //  </ChannelPackageType>
            //</ArrayOfChannelPackageType>

            var packagesInApMax = RetrieveAllChannelLineups();

            foreach (var channelPackageType in channelPackageTypes)
            {
                if (String.IsNullOrWhiteSpace(channelPackageType.PackageID))
                {
                    channelPackageType.PackageID = (from p in packagesInApMax
                                                    from cp in p.ChannelPackages
                                                    where String.Equals(cp.PackageName.Trim(), channelPackageType.PackageName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                                    select cp.PackageID).FirstOrDefault();
                }
            }

            iptvAccountType.ChannelPackageList = channelPackageTypes.ToArray();
            iptvAccountType.SetTopBoxList = null;

            _iptvService.SetIPTVAccount(_loginInformation.LoginToken, iptvAccountType, iptvSubscriberType);
        }

        public void EnableIptvAccount(IPTVAccountType iptvAccountType, SubscriberType subscriberType)
        {
            iptvAccountType.Active = true;
            iptvAccountType.SetTopBoxList = null;

            if (iptvAccountType.PurchasePIN == null || iptvAccountType.PurchasePIN.Length < 4)
                iptvAccountType.PurchasePIN = "0000";

            if (iptvAccountType.PurchasePIN == null || iptvAccountType.PurchasePIN.Length < 4)
                iptvAccountType.RatingPIN = "0000";

            _iptvService.SetIPTVAccount(_loginInformation.LoginToken, iptvAccountType, subscriberType);
        }

        public void EnableIptvAccountByServiceReference(string serviceReference)
        {
            IPTVAccountType iptvAccountType;
            SubscriberType iptvSubscriberType;
            RetrieveIptvAccountTypeAndIptvSubscriberTypeByServiceReference(serviceReference, out iptvAccountType, out iptvSubscriberType);

            EnableIptvAccount(iptvAccountType, iptvSubscriberType);
        }

        //public void EnableIptvAccountBySubscriberDefaultPhoneNumberAndServiceReference(string subscriberDefaultPhoneNumber, string serviceReference)
        //{
        //    var myCurrentIptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);

        //    if (myCurrentIptvAccountType == null)
        //        throw new Exception("Error IPTX Account null. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

        //    if (myCurrentIptvAccountType.SubscriberID == null)
        //        throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

        //    if (myCurrentIptvAccountType.SubscriberID.Length == 0)
        //        throw new Exception("Error IPTVAccount.SubscriberID is 0. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

        //    //Get the existing subscriber info.
        //    var subscriberVersion = _versions.FirstOrDefault(x => x.Key.Equals(Enums.ApmaxVersion.Subscriber)).Value;
        //    SubscriberType myIptvSubscriberType;

        //    switch (subscriberVersion)
        //    {
        //        case 3:
        //            var apMaxSubscriberServiceV3 = new ApMaxSubscriberV3Service(_setting, _versions);
        //            SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByDefaultPhoneNumber(subscriberDefaultPhoneNumber);

        //            if (existingSubscriberTypeV3 == null)
        //                throw new Exception("Unable to find subscriber account with Guid: " + subscriberDefaultPhoneNumber + ".\r\n");

        //            //myIptvSubscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
        //            myIptvSubscriberType = existingSubscriberTypeV3.SerializeObjectToJsonString().DeserializeJsonStringToObject<SubscriberType>();

        //            break;
        //        case 4:
        //            var apMaxSubscriberServiceV4 = new ApMaxSubscriberV4Service(_setting, _versions);
        //            SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByDefaultPhoneNumber(subscriberDefaultPhoneNumber);

        //            if (existingSubscriberTypeV4 == null)
        //                throw new Exception("Unable to find subscriber account with Guid: " + subscriberDefaultPhoneNumber + ".\r\n");

        //            //myIptvSubscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
        //            myIptvSubscriberType = existingSubscriberTypeV4.SerializeObjectToJsonString().DeserializeJsonStringToObject<SubscriberType>();
        //            break;
        //        default:
        //            throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
        //    }

        //    EnableIptvAccount(myCurrentIptvAccountType, myIptvSubscriberType);
        //}

        //public void EnableIptvAccountBySubscriberGuidAndServiceReference(string subscriberGuid, string serviceReference)
        //{
        //    var mySubscriberGuid = Guid.Parse(subscriberGuid);

        //    var myCurrentIptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);

        //    if (myCurrentIptvAccountType == null)
        //        throw new Exception("Error IPTX Account null. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

        //    if (myCurrentIptvAccountType.SubscriberID == null)
        //        throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

        //    if (myCurrentIptvAccountType.SubscriberID.Length == 0)
        //        throw new Exception("Error IPTVAccount.SubscriberID is 0. Unable to find IPTV account with ServiceReference: " + serviceReference + ".\r\n");

        //    //Get the existing subscriber info.
        //    var subscriberVersion = _versions.FirstOrDefault(x => x.Key.Equals(Enums.ApmaxVersion.Subscriber)).Value;
        //    SubscriberType myIptvSubscriberType;

        //    switch (subscriberVersion)
        //    {
        //        case 3:
        //            var apMaxSubscriberServiceV3 = new ApMaxSubscriberV3Service(_setting, _versions);
        //            SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByGuid(mySubscriberGuid);

        //            if (existingSubscriberTypeV3 == null)
        //                throw new Exception("Unable to find subscriber account with Guid: " + subscriberGuid + ".\r\n");

        //            //myIptvSubscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
        //            myIptvSubscriberType = existingSubscriberTypeV3.SerializeObjectToJsonString().DeserializeJsonStringToObject<SubscriberType>();

        //            break;
        //        case 4:
        //            var apMaxSubscriberServiceV4 = new ApMaxSubscriberV4Service(_setting, _versions);
        //            SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByGuid(mySubscriberGuid);

        //            if (existingSubscriberTypeV4 == null)
        //                throw new Exception("Unable to find subscriber account with Guid: " + subscriberGuid + ".\r\n");

        //            //myIptvSubscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
        //            myIptvSubscriberType = existingSubscriberTypeV4.SerializeObjectToJsonString().DeserializeJsonStringToObject<SubscriberType>();
        //            break;
        //        default:
        //            throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
        //    }

        //    EnableIptvAccount(myCurrentIptvAccountType, myIptvSubscriberType);
        //}

        public void EnableIptvAccountBySubAddressAndServiceReference(string subAddress, string serviceReference)
        {
            subAddress = subAddress.PadLeft(10, '0');

            var myIptvAccountTypeArray = _iptvService.GetAccountsBySubAddress(_loginInformation.LoginToken, subAddress).ToArray();

            if (myIptvAccountTypeArray == null || !myIptvAccountTypeArray.Any())
                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + subAddress + ".\r\n");

            var myCurrentIptvAccountType = new IPTVAccountType();

            foreach (var myIptvAccountType in myIptvAccountTypeArray)
            {
                if (myIptvAccountType.ServiceReference.Trim().Equals(serviceReference.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    myCurrentIptvAccountType = myIptvAccountType;
                    break;
                }
            }

            if (myCurrentIptvAccountType == null)
                throw new Exception("Error IPTX Account null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID == null)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is 0. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            //Get the existing subscriber info.
            var subscriberVersion = _versions.Subscriber;
            SubscriberType myIptvSubscriberType;

            switch (subscriberVersion)
            {
                case 3:
                    var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                    SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByDefaultPhoneNumber(subAddress);

                    if (existingSubscriberTypeV3 == null)
                        throw new Exception("Unable to find subscriber account with this SubAddress: " + subAddress + ".\r\n");

                    myIptvSubscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                    break;
                case 4:
                    var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                    SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByDefaultPhoneNumber(subAddress);

                    if (existingSubscriberTypeV4 == null)
                        throw new Exception("Unable to find subscriber account with this SubAddress: " + subAddress + ".\r\n");

                    myIptvSubscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
                    break;
                default:
                    throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
            }

            EnableIptvAccount(myCurrentIptvAccountType, myIptvSubscriberType);
        }

        public void DisableIptvAccount(IPTVAccountType iptvAccountType, SubscriberType subscriberType)
        {
            iptvAccountType.Active = false;
            iptvAccountType.SetTopBoxList = null;

            if (iptvAccountType.PurchasePIN.Length < 4)
                iptvAccountType.PurchasePIN = "0000";

            if (iptvAccountType.RatingPIN.Length < 4)
                iptvAccountType.RatingPIN = "0000";

            _iptvService.SetIPTVAccount(_loginInformation.LoginToken, iptvAccountType, subscriberType);
        }

        public void SuspendIptvAccountBySubscriberId(Guid subscriberId, bool suspend, string suspendReason)
        {
            _iptvService.SuspendAccount(_loginInformation.LoginToken, subscriberId.ToString(), suspend, suspendReason);
        }

        public void SuspendIptvAccountByServiceReference(string serviceReference, bool suspend, string suspendReason)
        {
            if (string.IsNullOrEmpty(serviceReference))
            {
                throw new Exception("Must provide a valid Service Reference.");
            }

            var iptvAccount = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);

            if (iptvAccount == null || string.IsNullOrEmpty(iptvAccount.SubscriberID))
            {
                throw new Exception("Unable to retrieve IptvAccount via GetSubscriberAccount based on service reference of:" + serviceReference);
            }

            var subscriberId = iptvAccount.SubscriberID;

            SuspendIptvAccountBySubscriberId(Guid.Parse(subscriberId), suspend, suspendReason);
        }

        public SubscriberType RetrieveIptvSubscriberFromSubscriberGuid(Guid subscriberGuid)
        {
            SubscriberType subscriberType;

            //Get the existing subscriber info.
            var subscriberVersion = _versions.Subscriber;

            switch (subscriberVersion)
            {
                case 3:
                    var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                    SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByGuid(subscriberGuid);

                    if (existingSubscriberTypeV3 == null)
                        throw new Exception("Unable to find subscriber account with this SubAddress: " + subscriberGuid + ".\r\n");

                    subscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
                    return subscriberType;

                    break;
                case 4:
                    var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                    SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByGuid(subscriberGuid);

                    if (existingSubscriberTypeV4 == null)
                        throw new Exception("Unable to find subscriber account with this SubAddress: " + subscriberGuid + ".\r\n");

                    subscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
                    return subscriberType;

                    break;
                default:
                    throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
            }

        }

        public void RetrieveIptvAccountTypeAndIptvSubscriberTypeByServiceReference(string serviceReference, out IPTVAccountType iptvAccountType, out SubscriberType subscriberType)
        {
            iptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);

            if (iptvAccountType == null)
                throw new Exception("Error IPTV Account null. Unable to find IPTV account with Service Reference: " + serviceReference + ".\r\n");

            if (iptvAccountType.SubscriberID == null)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with Service Reference: " + serviceReference + ".\r\n");

            if (iptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is 0. Unable to find IPTV account with Service Reference: " + serviceReference + ".\r\n");

            subscriberType = RetrieveIptvSubscriberFromSubscriberGuid(Guid.Parse(iptvAccountType.SubscriberID));
        }

        public void DisableIptvAccountByServiceReference(string serviceReference)
        {
            IPTVAccountType iptvAccountType;
            SubscriberType myIptvSubscriberType;
            RetrieveIptvAccountTypeAndIptvSubscriberTypeByServiceReference(serviceReference, out iptvAccountType, out myIptvSubscriberType);

            DisableIptvAccount(iptvAccountType, myIptvSubscriberType);
        }

        public void DisableIptvAccountBySubAddressAndServiceReference(string subAddress, string serviceReference)
        {
            subAddress = subAddress.PadLeft(10, '0');

            var myIptvAccountTypeArray = _iptvService.GetAccountsBySubAddress(_loginInformation.LoginToken, subAddress);

            if (myIptvAccountTypeArray == null || !myIptvAccountTypeArray.Any())
                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + subAddress + ".\r\n");

            var myCurrentIptvAccountType = new IPTVAccountType();

            foreach (IPTVAccountType myIptvAccountType in myIptvAccountTypeArray)
            {
                if (myIptvAccountType.ServiceReference.Trim().Equals(serviceReference.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    myCurrentIptvAccountType = myIptvAccountType;
                    break;
                }
            }

            if (myCurrentIptvAccountType == null)
                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID == null)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is 0. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            //Get the existing subscriber info.
            var subscriberVersion = _versions.Subscriber;
            SubscriberType myIptvSubscriberType;

            switch (subscriberVersion)
            {
                case 3:
                    var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                    SubscriberV3.SubscriberType existingSubscriberTypeV3 = apMaxSubscriberServiceV3.RetrieveSubscriberByDefaultPhoneNumber(subAddress);

                    if (existingSubscriberTypeV3 == null)
                        throw new Exception("Unable to find subscriber account with this SubAddress: " + subAddress + ".\r\n");

                    myIptvSubscriberType = existingSubscriberTypeV3.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();

                    break;
                case 4:
                    var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                    SubscriberV4.SubscriberType existingSubscriberTypeV4 = apMaxSubscriberServiceV4.RetrieveSubscriberByDefaultPhoneNumber(subAddress);

                    if (existingSubscriberTypeV4 == null)
                        throw new Exception("Unable to find subscriber account with this SubAddress: " + subAddress + ".\r\n");

                    myIptvSubscriberType = existingSubscriberTypeV4.SerializeObjectToString().DeSerializeStringToObject<SubscriberType>();
                    break;
                default:
                    throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
            }

            DisableIptvAccount(myCurrentIptvAccountType, myIptvSubscriberType);
        }

        public void DeleteIptvAccountBySubscriberGuidAndServiceReference(Guid subscriberGuid, string serviceReference, bool force = false)
        {
            if (force)
            {
                _iptvService.ForceDeleteIPTVAccount(_loginInformation.LoginToken, subscriberGuid.ToString(), serviceReference);
            }
            else
            {
                _iptvService.DeleteIPTVAccount(_loginInformation.LoginToken, subscriberGuid.ToString(), serviceReference);
            }
        }

        public void DeleteIptvByServiceReference(string serviceReference, bool force = false)
        {
            var iptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);

            if (iptvAccountType == null)
            {
                throw new Exception("Unable to retrieve IPTV Account Type by Service Reference: " + serviceReference + ".");
            }

            if (string.IsNullOrEmpty(iptvAccountType.SubscriberID))
            {
                throw new Exception("Unable to retrieve IPTV Account Type Subscriber Id by Service Reference: " + serviceReference + ".");
            }

            var subscriberGuid = Guid.Parse(iptvAccountType.SubscriberID);

            DeleteIptvAccountBySubscriberGuidAndServiceReference(subscriberGuid, serviceReference, force);
        }

        public void DeleteIptvByBillingAccountAndServiceReference(string billingAccount, string serviceReference, bool force = false)
        {
            var iptvAccountTypes = _iptvService.GetSubscriberAccountsByBilling(_loginInformation.LoginToken, billingAccount);

            IPTVAccountType myCurrentIptvAccountType = iptvAccountTypes.FirstOrDefault(iptvAccountType => iptvAccountType.ServiceReference == serviceReference);

            if (myCurrentIptvAccountType == null)
            {
                throw new Exception("Unable to retrieve IPTV Account Type by billing account: " + billingAccount + ".");
            }

            if (string.IsNullOrEmpty(myCurrentIptvAccountType.SubscriberID))
            {
                throw new Exception("Unable to retrieve IPTV Account Type Subscriber Id by billing account: " + billingAccount + ".");
            }

            var subscriberGuid = Guid.Parse(myCurrentIptvAccountType.SubscriberID);
            DeleteIptvAccountBySubscriberGuidAndServiceReference(subscriberGuid, serviceReference, force);
        }

        public void DeleteIptvAccountBySubAddressAndServiceReference(string subAddress, string serviceReference, bool deleteSubscriber)
        {
            subAddress = subAddress.PadLeft(10, '0');

            var myIptvAccountTypeArray = _iptvService.GetAccountsBySubAddress(_loginInformation.LoginToken, subAddress);

            if (myIptvAccountTypeArray == null || !myIptvAccountTypeArray.Any())
                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + subAddress + ".\r\n");

            IPTVAccountType myCurrentIptvAccountType = null;

            foreach (var myIptvAccountType in myIptvAccountTypeArray)
            {
                if (myIptvAccountType.ServiceReference == serviceReference)
                {
                    myCurrentIptvAccountType = myIptvAccountType;
                    break;
                }
            }

            if (myCurrentIptvAccountType == null)
                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID == null)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is 0. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            _iptvService.DeleteIPTVAccount(_loginInformation.LoginToken, myCurrentIptvAccountType.SubscriberID, serviceReference);

            if (deleteSubscriber)
            {
                var subscriberVersion = _versions.Subscriber;

                switch (subscriberVersion)
                {
                    case 3:
                        var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                        apMaxSubscriberServiceV3.DeleteSubscriberByGuid(Guid.Parse(myCurrentIptvAccountType.SubscriberID));
                        break;
                    case 4:
                        var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                        apMaxSubscriberServiceV4.DeleteSubscriberByGuid(Guid.Parse(myCurrentIptvAccountType.SubscriberID));
                        break;
                    default:
                        throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
                }
            }
        }

        public void ForceDeleteIptvAccount(string subAddress, string serviceReference, bool deleteSubscriber)
        {
            subAddress = subAddress.PadLeft(10, '0');

            var myIptvAccountTypeArray = _iptvService.GetAccountsBySubAddress(_loginInformation.LoginToken, subAddress);

            if (myIptvAccountTypeArray == null || !myIptvAccountTypeArray.Any())
                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + subAddress + ".\r\n");

            IPTVAccountType myCurrentIptvAccountType = null;

            foreach (var myIptvAccountType in myIptvAccountTypeArray)
            {
                if (myIptvAccountType.ServiceReference == serviceReference)
                {
                    myCurrentIptvAccountType = myIptvAccountType;
                    break;
                }
            }

            if (myCurrentIptvAccountType == null)
                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID == null)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            if (myCurrentIptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is 0. Unable to find IPTV account with this SubAddress: " + subAddress + " and this ServiceReference: " + serviceReference + ".\r\n");

            _iptvService.ForceDeleteIPTVAccount(_loginInformation.LoginToken, myCurrentIptvAccountType.SubscriberID, serviceReference);

            if (deleteSubscriber)
            {
                var subscriberVersion = _versions.Subscriber;

                switch (subscriberVersion)
                {
                    case 3:
                        var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                        apMaxSubscriberServiceV3.DeleteSubscriberByGuid(Guid.Parse(myCurrentIptvAccountType.SubscriberID));
                        break;
                    case 4:
                        var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                        apMaxSubscriberServiceV4.DeleteSubscriberByGuid(Guid.Parse(myCurrentIptvAccountType.SubscriberID));
                        break;
                    default:
                        throw new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented.");
                }
            }
        }

        public void DeleteStbFromIptvAccount(string macAddress, string subscriberId = "", string serviceReference = "")
        {
            if (string.IsNullOrEmpty(macAddress))
            {
                throw new Exception("DeleteStbFromIptvAccount: Must provide a Mac Address");
            }

            IPTVAccountType iptvAccountType;
            if (!string.IsNullOrEmpty(subscriberId))
            {
                iptvAccountType = _iptvService.GetSubscriberAccountBySubscriberId(_loginInformation.LoginToken, serviceReference);
            }
            else if (!string.IsNullOrEmpty(serviceReference))
            {
                iptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);
            }
            else
            {
                throw new Exception("DeleteStbFromIptvAccount: Must provide either a Subscriber Id or a Service Reference.");
            }

            if (iptvAccountType == null)
                throw new Exception("Error IPTV Account null. Unable to find IPTV account. Mac Address (" + macAddress + "), Subscriber Id (" + subscriberId + "), Service Reference (" + serviceReference + ")." + Environment.NewLine);

            if (iptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account. Mac Address (" + macAddress + "), Subscriber Id (" + subscriberId + "), Service Reference (" + serviceReference + ")." + Environment.NewLine);

            _iptvService.DeleteStb(_loginInformation.LoginToken, iptvAccountType.SubscriberID, macAddress);
        }

        public void DeauthorizeStb(string macAddress, string subscriberId = "", string serviceReference = "")
        {

            if (string.IsNullOrEmpty(macAddress))
            {
                throw new Exception("DeleteStbFromIptvAccount: Must provide a Mac Address");
            }

            IPTVAccountType iptvAccountType;
            if (!string.IsNullOrEmpty(subscriberId))
            {
                iptvAccountType = _iptvService.GetSubscriberAccountBySubscriberId(_loginInformation.LoginToken, serviceReference);
            }
            else if (!string.IsNullOrEmpty(serviceReference))
            {
                iptvAccountType = _iptvService.GetSubscriberAccount(_loginInformation.LoginToken, serviceReference);
            }
            else
            {
                throw new Exception("DeleteStbFromIptvAccount: Must provide either a Subscriber Id or a Service Reference.");
            }

            if (iptvAccountType == null)
                throw new Exception("Error IPTV Account null. Unable to find IPTV account. Mac Address (" + macAddress + "), Subscriber Id (" + subscriberId + "), Service Reference (" + serviceReference + ")." + Environment.NewLine);

            if (iptvAccountType.SubscriberID.Length == 0)
                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account. Mac Address (" + macAddress + "), Subscriber Id (" + subscriberId + "), Service Reference (" + serviceReference + ")." + Environment.NewLine);


            _iptvService.DeauthorizeStb(_loginInformation.LoginToken, iptvAccountType.SubscriberID, macAddress);
        }

        private List<IPTVAccountType> FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(List<IPTVAccountType> iptvAccountTypes)
        {
            bool isMissingPackageNames = false;

            //Having issues with nulls... So explicitly checking for nulls.
            if (iptvAccountTypes != null && iptvAccountTypes.Any())
            {
                foreach (var iptvAccountType in iptvAccountTypes)
                {
                    if (iptvAccountType != null && iptvAccountType.ChannelPackageList != null && iptvAccountType.ChannelPackageList.Any())
                    {
                        isMissingPackageNames = (
                            from y in iptvAccountType.ChannelPackageList
                            where (y.PackageName == null || y.PackageName.Trim() == "")
                            select y
                            ).Any();
                    }
                }
            }

            if (isMissingPackageNames)
            {
                iptvAccountTypes = PopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);
            }

            return iptvAccountTypes;
        }

        private List<IPTVAccountType> PopulateMissingPackageNamesInIptvAccountTypeClass(List<IPTVAccountType> iptvAccountTypes)
        {
            var channelLineups = RetrieveAllChannelLineups();

            foreach (var iptvAccountType in iptvAccountTypes)
            {
                foreach (var channelPackage in iptvAccountType.ChannelPackageList)
                {
                    if (string.IsNullOrEmpty(channelPackage.PackageName))
                    {
                        var packageName = (
                            from x in channelLineups
                            from y in x.ChannelPackages
                            where String.Equals(y.PackageID.Trim(), channelPackage.PackageID.Trim(), StringComparison.CurrentCultureIgnoreCase)
                            select y.PackageName
                            ).FirstOrDefault();

                        channelPackage.PackageName = packageName;
                    }
                }
            }

            return iptvAccountTypes;
        }

    }
}
