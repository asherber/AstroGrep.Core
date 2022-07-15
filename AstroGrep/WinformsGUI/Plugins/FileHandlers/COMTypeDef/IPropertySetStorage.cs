using System;
using System.Runtime.InteropServices;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
    internal interface IPropertySetStorage
    {
        void Create(ref Guid rfmtid, ref Guid pclsid, uint grfFlags, uint grfMode, out IPropertyStorage ppprstg);
        void Delete(ref Guid rfmtid);
        void Enum(out IEnumSTATPROPSETSTG ppenum);
        [PreserveSig]
        int Open(ref Guid rfmtid, uint grfMode, out IPropertyStorage ppprstg);
    }
}
