﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.CallLoggingV3 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CallLoggingType", Namespace="http://schemas.datacontract.org/2004/07/CallLoggingServiceV3")]
    [System.SerializableAttribute()]
    public partial class CallLoggingType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubscriberDnField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubscriberIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> WebPortalEnabledField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailAddress {
            get {
                return this.EmailAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailAddressField, value) != true)) {
                    this.EmailAddressField = value;
                    this.RaisePropertyChanged("EmailAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SubscriberDn {
            get {
                return this.SubscriberDnField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriberDnField, value) != true)) {
                    this.SubscriberDnField = value;
                    this.RaisePropertyChanged("SubscriberDn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SubscriberID {
            get {
                return this.SubscriberIDField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriberIDField, value) != true)) {
                    this.SubscriberIDField = value;
                    this.RaisePropertyChanged("SubscriberID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> WebPortalEnabled {
            get {
                return this.WebPortalEnabledField;
            }
            set {
                if ((this.WebPortalEnabledField.Equals(value) != true)) {
                    this.WebPortalEnabledField = value;
                    this.RaisePropertyChanged("WebPortalEnabled");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SubscriberType", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    [System.SerializableAttribute()]
    public partial class SubscriberType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.CallLoggingV3.AddressInfoType[] AddressesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BillingAccountNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BillingEnvironmentCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BillingServiceAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DialByNameDigitsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.CallLoggingV3.InternetAccessType InternetAccessField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastUpdateTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastUpdatedByField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LatitudeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LongitudeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParentSubscriberIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.CallLoggingV3.PlacementType_e PlacementTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.CallLoggingV3.ServiceInfoType ServiceInformationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubscriberDefaultPhoneNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubscriberEmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubscriberGuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubscriberNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.CallLoggingV3.Timezone_e SubscriberTimezoneField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.CallLoggingV3.AddressInfoType[] Addresses {
            get {
                return this.AddressesField;
            }
            set {
                if ((object.ReferenceEquals(this.AddressesField, value) != true)) {
                    this.AddressesField = value;
                    this.RaisePropertyChanged("Addresses");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BillingAccountNumber {
            get {
                return this.BillingAccountNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.BillingAccountNumberField, value) != true)) {
                    this.BillingAccountNumberField = value;
                    this.RaisePropertyChanged("BillingAccountNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BillingEnvironmentCode {
            get {
                return this.BillingEnvironmentCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.BillingEnvironmentCodeField, value) != true)) {
                    this.BillingEnvironmentCodeField = value;
                    this.RaisePropertyChanged("BillingEnvironmentCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BillingServiceAddress {
            get {
                return this.BillingServiceAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.BillingServiceAddressField, value) != true)) {
                    this.BillingServiceAddressField = value;
                    this.RaisePropertyChanged("BillingServiceAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DialByNameDigits {
            get {
                return this.DialByNameDigitsField;
            }
            set {
                if ((object.ReferenceEquals(this.DialByNameDigitsField, value) != true)) {
                    this.DialByNameDigitsField = value;
                    this.RaisePropertyChanged("DialByNameDigits");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.CallLoggingV3.InternetAccessType InternetAccess {
            get {
                return this.InternetAccessField;
            }
            set {
                if ((object.ReferenceEquals(this.InternetAccessField, value) != true)) {
                    this.InternetAccessField = value;
                    this.RaisePropertyChanged("InternetAccess");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastUpdateTime {
            get {
                return this.LastUpdateTimeField;
            }
            set {
                if ((object.ReferenceEquals(this.LastUpdateTimeField, value) != true)) {
                    this.LastUpdateTimeField = value;
                    this.RaisePropertyChanged("LastUpdateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastUpdatedBy {
            get {
                return this.LastUpdatedByField;
            }
            set {
                if ((object.ReferenceEquals(this.LastUpdatedByField, value) != true)) {
                    this.LastUpdatedByField = value;
                    this.RaisePropertyChanged("LastUpdatedBy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Latitude {
            get {
                return this.LatitudeField;
            }
            set {
                if ((object.ReferenceEquals(this.LatitudeField, value) != true)) {
                    this.LatitudeField = value;
                    this.RaisePropertyChanged("Latitude");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Longitude {
            get {
                return this.LongitudeField;
            }
            set {
                if ((object.ReferenceEquals(this.LongitudeField, value) != true)) {
                    this.LongitudeField = value;
                    this.RaisePropertyChanged("Longitude");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ParentSubscriberId {
            get {
                return this.ParentSubscriberIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ParentSubscriberIdField, value) != true)) {
                    this.ParentSubscriberIdField = value;
                    this.RaisePropertyChanged("ParentSubscriberId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.CallLoggingV3.PlacementType_e PlacementType {
            get {
                return this.PlacementTypeField;
            }
            set {
                if ((this.PlacementTypeField.Equals(value) != true)) {
                    this.PlacementTypeField = value;
                    this.RaisePropertyChanged("PlacementType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.CallLoggingV3.ServiceInfoType ServiceInformation {
            get {
                return this.ServiceInformationField;
            }
            set {
                if ((object.ReferenceEquals(this.ServiceInformationField, value) != true)) {
                    this.ServiceInformationField = value;
                    this.RaisePropertyChanged("ServiceInformation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SubscriberDefaultPhoneNumber {
            get {
                return this.SubscriberDefaultPhoneNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriberDefaultPhoneNumberField, value) != true)) {
                    this.SubscriberDefaultPhoneNumberField = value;
                    this.RaisePropertyChanged("SubscriberDefaultPhoneNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SubscriberEmail {
            get {
                return this.SubscriberEmailField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriberEmailField, value) != true)) {
                    this.SubscriberEmailField = value;
                    this.RaisePropertyChanged("SubscriberEmail");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SubscriberGuid {
            get {
                return this.SubscriberGuidField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriberGuidField, value) != true)) {
                    this.SubscriberGuidField = value;
                    this.RaisePropertyChanged("SubscriberGuid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SubscriberName {
            get {
                return this.SubscriberNameField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriberNameField, value) != true)) {
                    this.SubscriberNameField = value;
                    this.RaisePropertyChanged("SubscriberName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.CallLoggingV3.Timezone_e SubscriberTimezone {
            get {
                return this.SubscriberTimezoneField;
            }
            set {
                if ((this.SubscriberTimezoneField.Equals(value) != true)) {
                    this.SubscriberTimezoneField = value;
                    this.RaisePropertyChanged("SubscriberTimezone");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InternetAccessType", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    [System.SerializableAttribute()]
    public partial class InternetAccessType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool MobileEnabledField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ServiceEnabledField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool MobileEnabled {
            get {
                return this.MobileEnabledField;
            }
            set {
                if ((this.MobileEnabledField.Equals(value) != true)) {
                    this.MobileEnabledField = value;
                    this.RaisePropertyChanged("MobileEnabled");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ServiceEnabled {
            get {
                return this.ServiceEnabledField;
            }
            set {
                if ((this.ServiceEnabledField.Equals(value) != true)) {
                    this.ServiceEnabledField = value;
                    this.RaisePropertyChanged("ServiceEnabled");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceInfoType", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    [System.SerializableAttribute()]
    public partial class ServiceInfoType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApSystemIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BillingServiceAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BillingServiceIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ServiceGuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.CallLoggingV3.ServiceType_e ServiceTypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApSystemId {
            get {
                return this.ApSystemIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ApSystemIdField, value) != true)) {
                    this.ApSystemIdField = value;
                    this.RaisePropertyChanged("ApSystemId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BillingServiceAddress {
            get {
                return this.BillingServiceAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.BillingServiceAddressField, value) != true)) {
                    this.BillingServiceAddressField = value;
                    this.RaisePropertyChanged("BillingServiceAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BillingServiceID {
            get {
                return this.BillingServiceIDField;
            }
            set {
                if ((object.ReferenceEquals(this.BillingServiceIDField, value) != true)) {
                    this.BillingServiceIDField = value;
                    this.RaisePropertyChanged("BillingServiceID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ServiceGuid {
            get {
                return this.ServiceGuidField;
            }
            set {
                if ((object.ReferenceEquals(this.ServiceGuidField, value) != true)) {
                    this.ServiceGuidField = value;
                    this.RaisePropertyChanged("ServiceGuid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.CallLoggingV3.ServiceType_e ServiceType {
            get {
                return this.ServiceTypeField;
            }
            set {
                if ((this.ServiceTypeField.Equals(value) != true)) {
                    this.ServiceTypeField = value;
                    this.RaisePropertyChanged("ServiceType");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AddressInfoType", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    [System.SerializableAttribute()]
    public partial class AddressInfoType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressFieldField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.CallLoggingV3.AddressType_e AddressTypeFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AddressField {
            get {
                return this.AddressFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.AddressFieldField, value) != true)) {
                    this.AddressFieldField = value;
                    this.RaisePropertyChanged("AddressField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.CallLoggingV3.AddressType_e AddressTypeField {
            get {
                return this.AddressTypeFieldField;
            }
            set {
                if ((this.AddressTypeFieldField.Equals(value) != true)) {
                    this.AddressTypeFieldField = value;
                    this.RaisePropertyChanged("AddressTypeField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PlacementType_e", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    public enum PlacementType_e : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PlacementType_None = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PlacementType_CASS = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PlacementType_User = 10,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Timezone_e", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    public enum Timezone_e : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ApDefault = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Midway = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Hawaii = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Alaska = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PacificTime = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Arizona = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MountainTime = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CentralTime = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Saskatchewan = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EasternTime = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IndianaEast = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AtlanticTime = 11,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Newfoundland = 12,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AddressType_e", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    public enum AddressType_e : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AddressTypeDN = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AddressTypeAnnID = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AddressTypeEmailAddr = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AddressTypeSipAddr = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AddressTypMailboxNumber = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AddressTypCentrexExtension = 15,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceType_e", Namespace="http://schemas.datacontract.org/2004/07/APmaxProvisioning")]
    public enum ServiceType_e : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unknown = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Voicemail = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Iptv = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LargeScaleConferencing = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OnDemandConferencing = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TerminatingCallManagement = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OriginatingCallManagement = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UniversalCallManagement = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CallLogging = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SipAcs = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WirelessOta = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ACDAgent = 11,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ACDCallCenter = 12,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SingleNumber = 13,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Notify = 14,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SipIntercom = 15,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IptvAluMmig = 16,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IptvMediaroom = 17,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        VoicemailMobile = 18,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IptvMobile = 19,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotifyMobile = 20,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CallLoggingV3.ICLPService")]
    public interface ICLPService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/FindCallLoggingSubscribers", ReplyAction="http://tempuri.org/ICLPService/FindCallLoggingSubscribersResponse")]
        Common.CallLoggingV3.CallLoggingType[] FindCallLoggingSubscribers(string loginToken, string searchNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/FindCallLoggingSubscribers", ReplyAction="http://tempuri.org/ICLPService/FindCallLoggingSubscribersResponse")]
        System.Threading.Tasks.Task<Common.CallLoggingV3.CallLoggingType[]> FindCallLoggingSubscribersAsync(string loginToken, string searchNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/AddClpSubscriberRecord", ReplyAction="http://tempuri.org/ICLPService/AddClpSubscriberRecordResponse")]
        void AddClpSubscriberRecord(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.SubscriberType subscriber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/AddClpSubscriberRecord", ReplyAction="http://tempuri.org/ICLPService/AddClpSubscriberRecordResponse")]
        System.Threading.Tasks.Task AddClpSubscriberRecordAsync(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.SubscriberType subscriber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/AddClpSubscriberRecordV2", ReplyAction="http://tempuri.org/ICLPService/AddClpSubscriberRecordV2Response")]
        void AddClpSubscriberRecordV2(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/AddClpSubscriberRecordV2", ReplyAction="http://tempuri.org/ICLPService/AddClpSubscriberRecordV2Response")]
        System.Threading.Tasks.Task AddClpSubscriberRecordV2Async(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/UpdateClpSubscriberRecord", ReplyAction="http://tempuri.org/ICLPService/UpdateClpSubscriberRecordResponse")]
        void UpdateClpSubscriberRecord(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/UpdateClpSubscriberRecord", ReplyAction="http://tempuri.org/ICLPService/UpdateClpSubscriberRecordResponse")]
        System.Threading.Tasks.Task UpdateClpSubscriberRecordAsync(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/RemoveClpSubscriberRecord", ReplyAction="http://tempuri.org/ICLPService/RemoveClpSubscriberRecordResponse")]
        void RemoveClpSubscriberRecord(string loginToken, string phoneNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICLPService/RemoveClpSubscriberRecord", ReplyAction="http://tempuri.org/ICLPService/RemoveClpSubscriberRecordResponse")]
        System.Threading.Tasks.Task RemoveClpSubscriberRecordAsync(string loginToken, string phoneNumber);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICLPServiceChannel : Common.CallLoggingV3.ICLPService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CLPServiceClient : System.ServiceModel.ClientBase<Common.CallLoggingV3.ICLPService>, Common.CallLoggingV3.ICLPService {
        
        public CLPServiceClient() {
        }
        
        public CLPServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CLPServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CLPServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CLPServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Common.CallLoggingV3.CallLoggingType[] FindCallLoggingSubscribers(string loginToken, string searchNumber) {
            return base.Channel.FindCallLoggingSubscribers(loginToken, searchNumber);
        }
        
        public System.Threading.Tasks.Task<Common.CallLoggingV3.CallLoggingType[]> FindCallLoggingSubscribersAsync(string loginToken, string searchNumber) {
            return base.Channel.FindCallLoggingSubscribersAsync(loginToken, searchNumber);
        }
        
        public void AddClpSubscriberRecord(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.SubscriberType subscriber) {
            base.Channel.AddClpSubscriberRecord(loginToken, clp, subscriber);
        }
        
        public System.Threading.Tasks.Task AddClpSubscriberRecordAsync(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.SubscriberType subscriber) {
            return base.Channel.AddClpSubscriberRecordAsync(loginToken, clp, subscriber);
        }
        
        public void AddClpSubscriberRecordV2(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo) {
            base.Channel.AddClpSubscriberRecordV2(loginToken, clp, internetInfo);
        }
        
        public System.Threading.Tasks.Task AddClpSubscriberRecordV2Async(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo) {
            return base.Channel.AddClpSubscriberRecordV2Async(loginToken, clp, internetInfo);
        }
        
        public void UpdateClpSubscriberRecord(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo) {
            base.Channel.UpdateClpSubscriberRecord(loginToken, clp, internetInfo);
        }
        
        public System.Threading.Tasks.Task UpdateClpSubscriberRecordAsync(string loginToken, Common.CallLoggingV3.CallLoggingType clp, Common.CallLoggingV3.InternetAccessType internetInfo) {
            return base.Channel.UpdateClpSubscriberRecordAsync(loginToken, clp, internetInfo);
        }
        
        public void RemoveClpSubscriberRecord(string loginToken, string phoneNumber) {
            base.Channel.RemoveClpSubscriberRecord(loginToken, phoneNumber);
        }
        
        public System.Threading.Tasks.Task RemoveClpSubscriberRecordAsync(string loginToken, string phoneNumber) {
            return base.Channel.RemoveClpSubscriberRecordAsync(loginToken, phoneNumber);
        }
    }
}
