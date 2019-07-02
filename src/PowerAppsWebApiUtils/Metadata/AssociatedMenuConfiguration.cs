using System;

namespace Microsoft.Dynamics.CRM
{   
        
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/associatedmenubehavior?view=dynamics-ce-odata-9"/>
    public enum AssociatedMenuBehavior
    {
        UseCollectionName = 0,	//Use the collection name for the associated menu.
        UseLabel = 1,	//Use the label for the associated menu.
        DoNotDisplay = 2,	//Do not show the associated menu.
    }

    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/associatedmenugroup?view=dynamics-ce-odata-9"/>
    public enum AssociatedMenuGroup
    {
        Details	= 0, //Show the associated menu in the details group.
        Sales	= 1,	//Show the associated menu in the sales group.
        Service	= 2,	//Show the associated menu in the service group.
        Marketing = 3,	//Show the associated menu in the marketing group.
    }


    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/associatedmenuconfiguration?view=dynamics-ce-odata-9"/>
    public sealed class AssociatedMenuConfiguration
    {
        public AssociatedMenuBehavior Behavior { get; set; }	
        //The behavior of the associated menu for an entity relationship.

        public AssociatedMenuGroup Group { get; set; }	
        //The structure that contains extra data.

        public Label Label { get; set; }	
        //The label for the associated menu.

        public int? Order { get; set; }
        //The order for the associated menu.

        public bool IsCustomizable { get; set; }	
        public string Icon { get; set; }
        public Guid ViewId { get; set; }	
        public bool AvailableOffline { get; set; }	
        public string MenuId { get; set; }
        public string QueryApi { get; set; }        
    }
}