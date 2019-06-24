using System;

namespace Microsoft.Dynamics.CRM
{   

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/cascadetype?view=dynamics-ce-odata-9"/>
    public enum CascadeType
    {
        NoCascade	= 0, //Do nothing.
        Cascade	= 1, //Perform the action on all referencing entity records associated with the referenced entity record.
        Active	= 2, //Perform the action on all active referencing entity records associated with the referenced entity record.
        UserOwned	= 3, //Perform the action on all referencing entity records owned by the same user as the referenced entity record.
        RemoveLink	= 4, //Remove the value of the referencing attribute for all referencing entity records associated with the referenced entity record.
        Restrict	= 5, //Prevent the Referenced entity record from being deleted when referencing entities exist.
    }


    
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/cascadeconfiguration?view=dynamics-ce-odata-9"/>
    public sealed class CascadeConfiguration
    {
        //The referenced entity record owner is changed.
        public CascadeType Assign { get; set; }	

        //The referenced entity record is deleted.
        public CascadeType Delete { get; set; }	

        //The record is merged with another record.
        public CascadeType Merge { get; set; }	

        //The referencing attribute in a parental relationship changes
        public CascadeType Reparent { get; set; }	

        //The referenced entity record is shared with another user.
        public CascadeType Share { get; set; }	

        //Sharing is removed for the referenced entity record.
        public CascadeType Unshare { get; set; }	

        //Indicates that the associated activities of the related entity should be included in Activity Associated View for the primary entity.	
        public CascadeType RollupView { get; set; }	
    }
}