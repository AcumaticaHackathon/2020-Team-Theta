using System;
using PX.Data;

namespace PX.FirefliesConnector.Ext
{
    [Serializable]
    [PXCacheName("TransScriptDetail")]
    public class TransScriptDetail : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        //#region Noteid
        //[PXGuid(IsKey =true)]
        //public virtual Guid? Noteid { get; set; }
        //public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        //#endregion

        #region Id
        [PXString(256, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "ID")]
        public virtual string Id { get; set; }
        public abstract class id : PX.Data.BQL.BqlString.Field<id> { }
        #endregion

        #region Title
        [PXString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Title")]
        public virtual string Title { get; set; }
        public abstract class title : PX.Data.BQL.BqlString.Field<title> { }
        #endregion

        #region Firefliesusers
        [PXString(512, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Firefliesusers")]
        public virtual string Firefliesusers { get; set; }
        public abstract class firefliesusers : PX.Data.BQL.BqlString.Field<firefliesusers> { }
        #endregion

        #region Date
        [PXDate()]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Duration
        [PXString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Duration")]
        public virtual string Duration { get; set; }
        public abstract class duration : PX.Data.BQL.BqlString.Field<duration> { }
        #endregion

        #region Transcripturl
        [PXString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transcript URL")]
        public virtual string Transcripturl { get; set; }
        public abstract class transcripturl : PX.Data.BQL.BqlString.Field<transcripturl> { }
        #endregion

        #region Participants
        [PXString(512, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Participants")]
        public virtual string Participants { get; set; }
        public abstract class participants : PX.Data.BQL.BqlString.Field<participants> { }
        #endregion

        #region Index
        [PXInt(IsKey = true)]
        [PXUIField(DisplayName = "Index")]
        public virtual int? Index { get; set; }
        public abstract class index : PX.Data.BQL.BqlInt.Field<index> { }
        #endregion

        #region Text
        [PXString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Text")]
        public virtual string Text { get; set; }
        public abstract class text : PX.Data.BQL.BqlString.Field<text> { }
        #endregion

        #region Rawtext
        [PXString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Rawtext")]
        public virtual string Rawtext { get; set; }
        public abstract class rawtext : PX.Data.BQL.BqlString.Field<rawtext> { }
        #endregion

        #region Starttime
        [PXString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Starttime")]
        public virtual string Starttime { get; set; }
        public abstract class starttime : PX.Data.BQL.BqlString.Field<starttime> { }
        #endregion

        #region Endtime
        [PXString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Endtime")]
        public virtual string Endtime { get; set; }
        public abstract class endtime : PX.Data.BQL.BqlString.Field<endtime> { }
        #endregion

        #region Speakerid
        [PXString(128, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Speakerid")]
        public virtual string Speakerid { get; set; }
        public abstract class speakerid : PX.Data.BQL.BqlString.Field<speakerid> { }
        #endregion

        #region minutes_consumed
        public abstract class minutesConsumed : PX.Data.BQL.BqlInt.Field<minutesConsumed>
        {
        }

        [PXInt()]
        [PXUIField(DisplayName = "Minutes Consumed")]
        public int? MinutesConsumed { get; set; }

        #endregion
    }
}