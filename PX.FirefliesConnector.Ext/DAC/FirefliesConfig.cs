using System;
using PX.Data;

namespace PX.FirefliesConnector.Ext
{
    [Serializable]
    [PXCacheName("Fireflies Configuration")]
    public class FirefliesConfig : IBqlTable
    {
        #region APIEndPoint
        public abstract class aPIEndPoint : PX.Data.BQL.BqlString.Field<aPIEndPoint> { }

        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "End Point")]
        public virtual string APIEndPoint { get; set; }
        #endregion

        #region Apikey
        public abstract class apikey : PX.Data.BQL.BqlString.Field<apikey> { }
        [PXDBString(400, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Auth Key")]
        public virtual string Apikey { get; set; }
        #endregion
    }
}