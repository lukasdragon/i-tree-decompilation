// Decompiled with JetBrains decompiler
// Type: ADOX._Catalog
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADOX
{
  [CompilerGenerated]
  [Guid("00000603-0000-0010-8000-00AA006D2EA4")]
  [DefaultMember("Tables")]
  [TypeIdentifier]
  [ComImport]
  public interface _Catalog
  {
    [DispId(0)]
    Tables Tables { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_7();

    [DispId(6)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    object Create([MarshalAs(UnmanagedType.BStr), In] string ConnectString);
  }
}
