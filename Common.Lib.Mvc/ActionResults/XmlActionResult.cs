using System.Web.Mvc;
using Common.Lib.Extensions;

namespace Common.Lib.MVC.ActionResults
{
    public class XmlActionResult : PartialViewResult
    {
        private readonly object _objectToSerialize;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlActionResult" /> class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        public XmlActionResult(object objectToSerialize)
        {
            _objectToSerialize = objectToSerialize;
        }

        /// <summary>
        /// Gets the object to be serialized to XML.
        /// </summary>
        public object ObjectToSerialize
        {
            get { return _objectToSerialize; }
        }

        /// <summary>
        /// Serializes the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (_objectToSerialize != null)
            {
                context.HttpContext.Response.Clear();
                var stringXml = _objectToSerialize.SerializeObjectToString();
                context.HttpContext.Response.ContentType = "text/xml";
                context.HttpContext.Response.Write(stringXml);
            }
        }
    }
}