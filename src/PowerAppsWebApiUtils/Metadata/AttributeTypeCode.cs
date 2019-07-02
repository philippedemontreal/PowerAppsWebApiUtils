using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.CRM
{
    ///<chref="https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/attributetypecode?view=dynamics-ce-odata-9" />
    public enum AttributeTypeCode
    {
        Boolean = 	0, //	A Boolean attribute.
        Customer = 	1, //	An attribute that represents a customer.
        DateTime = 	2, //	A date/time attribute.
        Decimal = 	3, //	A decimal attribute.
        Double = 	4, //	A double attribute.
        Integer = 	5, //	An integer attribute.
        Lookup = 	6, //	A lookup attribute.
        Memo = 	7, //	A memo attribute.
        Money = 	8, //	A money attribute.
        Owner = 	9, //	An owner attribute.
        PartyList = 	10, //	A partylist attribute.
        Picklist = 	11, //	A picklist attribute.
        State = 	12, //	A state attribute.
        Status = 	13, //	A status attribute.
        String = 	14, //	A string attribute.
        Uniqueidentifier = 	15, //	An attribute that is an ID.
        CalendarRules = 	16, //	An attribute that contains calendar rules.
        Virtual = 	17, //	An attribute that is created by the system at run time.
        BigInt = 	18, //	A big integer attribute.
        ManagedProperty = 	19, //	A managed property attribute.
        EntityName = 	20, //	An entity name attribute.
    }
}