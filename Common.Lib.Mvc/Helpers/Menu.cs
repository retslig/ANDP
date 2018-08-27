using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lib.MVC.Helpers
{
    public class Menu
    {
        public string GroupName { get; set; }
        public string Controller { get; set; }
        public string DisplayName { get; set; }
    }

    public static class MenuItems
    {
        public static List<Menu> Items
        {
            get
            {
                var items = new List<Menu>
                {
                    new Menu()
                    {
                        Controller = "Equipment",
                        DisplayName = "Equipment Settings",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "Equipment",
                        DisplayName = "Equipment Connections",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "Scripting",
                        DisplayName = "Data Dictionary",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "Scripting",
                        DisplayName = "Feature to Command Translation",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "General",
                        DisplayName = "Company",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "General",
                        DisplayName = "Languages",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "Engine",
                        DisplayName = "Engine Settings",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "License",
                        DisplayName = "License Settings",
                        GroupName = "Configuration"
                    },
                    new Menu()
                    {
                        Controller = "Order",
                        DisplayName = "Order Results",
                        GroupName = "Logs"
                    },
                    new Menu()
                    {
                        Controller = "Services",
                        DisplayName = "Services Results",
                        GroupName = "Logs"
                    },
                    new Menu()
                    {
                        Controller = "Items",
                        DisplayName = "Item Results",
                        GroupName = "Logs"
                    },
                    new Menu()
                    {
                        Controller = "BillingSync",
                        DisplayName = "Billing Sync",
                        GroupName = "Logs"
                    },
                    new Menu()
                    {
                        Controller = "Reporting",
                        DisplayName = "Reporting",
                        GroupName = "Logs"
                    },
                    new Menu()
                    {
                        Controller = "OrderViewer",
                        DisplayName = "View Orders",
                        GroupName = "Provisioning"
                    },
                    new Menu()
                    {
                        Controller = "BillingSync",
                        DisplayName = "Billing Sync",
                        GroupName = "Provisioning"
                    },
                    new Menu()
                    {
                        Controller = "Audits",
                        DisplayName = "Audits",
                        GroupName = "Provisioning"
                    },
                    new Menu()
                    {
                        Controller = "Promotions",
                        DisplayName = "Time Based Promotions",
                        GroupName = "Provisioning"
                    },
                    new Menu()
                    {
                        Controller = "Schedule",
                        DisplayName = "Schedule Settings",
                        GroupName = "Provisioning"
                    },
                    new Menu()
                    {
                        Controller = "Orders",
                        DisplayName = "Currently Processing",
                        GroupName = "Health"
                    },
                    new Menu()
                    {
                        Controller = "Orders",
                        DisplayName = "fail Over",
                        GroupName = "Health"
                    },
                    new Menu()
                    {
                        Controller = "Orders",
                        DisplayName = "Documentation",
                        GroupName = "Support"
                    },
                    new Menu()
                    {
                        Controller = "Orders",
                        DisplayName = "Help",
                        GroupName = "Support"
                    },
                    new Menu()
                    {
                        Controller = "Orders",
                        DisplayName = "Contact",
                        GroupName = "Support"
                    },
                };

                return items;
            }
        }
    }
}


//1.	Configuration
//a.	Security - Group Activities, USer Activities
//b.	Equipment - Equipment, Equipment connections
//c.	Scripting - Data Dictionary, Feature to Command Translation
//d.	General - Company, LAnguages
//e.	Engine - Settings
//f.	License
//2.	Logs
// .	Results - Orders, services, equipment
//a.	Statistics - Orders, services, equipment
//b.	Reporting
//3.	Orders
// .	View - Depending on permissions can instant provision, test mode, Undo
//a.	Billing Sync
//b.	Audits
//c.	Promotions - Time based.
//4.	Health
// .	Schedule - Start/stop
//a.	currently processing items
//b.	heart beat
//c.	load balancing
//d.	fail over
//5.	Support
// .	Help
//a.	Documentation
//b.	Contact
