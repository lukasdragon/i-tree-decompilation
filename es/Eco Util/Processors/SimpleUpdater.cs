// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.SimpleUpdater
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using NHibernate;

namespace Eco.Util.Processors
{
  public class SimpleUpdater : Updater
  {
    private PlotProcessor _plotProcessor;
    private PlotGroundCoverProcessor _pgcProcessor;
    private PlotLandUseProcessor _pluProcessor;
    private ReferenceObjectProcessor _roProcessor;
    private ShrubProcessor _shrubProcessor;
    private TreeProcessor _treeProcessor;
    private BuildingProcessor _buildingProcessor;
    private StemProcessor _stemProcessor;

    public SimpleUpdater(ISessionFactory sf)
      : base(sf)
    {
    }

    public override PlotProcessor PlotProcessor
    {
      get
      {
        if (this._plotProcessor == null)
          this._plotProcessor = new PlotProcessor((Updater) this);
        return this._plotProcessor;
      }
    }

    public override PlotGroundCoverProcessor PlotGroundCoverProcessor
    {
      get
      {
        if (this._pgcProcessor == null)
          this._pgcProcessor = new PlotGroundCoverProcessor((Updater) this);
        return this._pgcProcessor;
      }
    }

    public override PlotLandUseProcessor PlotLandUseProcessor
    {
      get
      {
        if (this._pluProcessor == null)
          this._pluProcessor = new PlotLandUseProcessor((Updater) this);
        return this._pluProcessor;
      }
    }

    public override ReferenceObjectProcessor ReferenceObjectProcessor
    {
      get
      {
        if (this._roProcessor == null)
          this._roProcessor = new ReferenceObjectProcessor((Updater) this);
        return this._roProcessor;
      }
    }

    public override ShrubProcessor ShrubProcessor
    {
      get
      {
        if (this._shrubProcessor == null)
          this._shrubProcessor = new ShrubProcessor((Updater) this);
        return this._shrubProcessor;
      }
    }

    public override TreeProcessor TreeProcessor
    {
      get
      {
        if (this._treeProcessor == null)
          this._treeProcessor = new TreeProcessor((Updater) this);
        return this._treeProcessor;
      }
    }

    public override BuildingProcessor BuildingProcessor
    {
      get
      {
        if (this._buildingProcessor == null)
          this._buildingProcessor = new BuildingProcessor((Updater) this);
        return this._buildingProcessor;
      }
    }

    public override StemProcessor StemProcessor
    {
      get
      {
        if (this._stemProcessor == null)
          this._stemProcessor = new StemProcessor((Updater) this);
        return this._stemProcessor;
      }
    }
  }
}
