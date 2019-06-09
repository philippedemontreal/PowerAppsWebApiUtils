using System;

namespace app.Entities
{
    public enum PrivilegeType
    {
        Share,
        Append,
        Assign,
        Delete,
        Write,
        Read,
        Create,
        AppendTo
    }
    public class Privilege
    {
        public bool CanBeBasic { get; set; }
		public bool CanBeDeep { get; set; }
		public bool CanBeGlobal { get; set; }
		public bool CanBeLocal { get; set; }
		public bool CanBeEntityReference { get; set; }
		public bool CanBeParentEntityReference { get; set; }
		public string Name { get; set; }
		public Guid PrivilegeId { get; set; }
		public PrivilegeType PrivilegeType { get; set; }
        
    }
}