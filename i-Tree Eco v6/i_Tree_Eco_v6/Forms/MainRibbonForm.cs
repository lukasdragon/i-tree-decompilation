// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.MainRibbonForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Zip;
using C1.Win.C1Ribbon;
using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.IO;
using Eco.Domain.Properties;
using Eco.Domain.v5;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Services;
using Eco.Util.Tasks;
using Eco.Util.Views;
using EcoIpedReportGenerator;
using i_Tree_Eco_v6.Engine;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Forms.Resources;
using i_Tree_Eco_v6.Interfaces;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Reports;
using i_Tree_Eco_v6.Reports.Chart;
using i_Tree_Eco_v6.SAS;
using i_Tree_Eco_v6.Tasks;
using LocationSpecies.Domain;
using Microsoft.Win32;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UFORE;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class MainRibbonForm : C1RibbonForm
  {
    private const short YEARS = 30;
    private const short FF_DAYS = 149;
    private const double DIEBACK_HEALTHY = 3.0;
    private const double DIEBACK_SICK = 13.08;
    private const double DIEBACK_DYING = 50.0;
    private const int SPLASH_TIME = 5000;
    private static SplashForm splashForm;
    private ProgramSession m_ps;
    private TaskManager m_taskManager;
    private HelpForm m_frmHelp;
    private LocationService _locationService;
    private short _ffDays;
    private RibbonTab m_curTab;
    private bool m_showSplashTabSplash;
    private bool m_tabChangeCanceled;
    private bool m_selectingProject;
    private bool m_closingDocuments;
    private string[] m_args;
    private bool m_showMetadataReport;
    private ISession m_session;
    private Eco.Domain.v6.Project m_project;
    private Eco.Domain.v6.Series m_series;
    private Year m_year;
    private Forecast m_forecast;
    private IList<Forecast> m_forecasts;
    private readonly object m_syncObj;
    private bool _locationValid;
    private Form currMap;
    private bool _windowSwitching;
    private bool _turnOffMdi;
    private IContainer components;
    private RibbonTrackBar statusTrackBar;
    private RibbonMenu viewsMenu;
    private RibbonToggleButton sbPrintLayoutToggleButton;
    private RibbonToggleButton sbFullScreenToggleButton;
    private RibbonToggleButton sbWebLayoutToggleButton;
    private RibbonLabel sbProectInfoLabel;
    private RibbonSeparator ribbonSeparator13;
    private C1StatusBar c1StatusBar1;
    private DockPanel dockPanel1;
    private RibbonTab rtSupport;
    private RibbonGroup ribbonGroup6;
    private RibbonButton manualButton;
    private RibbonButton checkForUpdatesButton;
    private RibbonButton visitITreeToolsDotOrgButton;
    private RibbonButton forumButton;
    private RibbonButton aboutButton;
    private RibbonTab rtReports;
    private RibbonGroup rgRunModels;
    private RibbonButton runModelsButton;
    private RibbonMenu rmBenefitsAndCosts;
    private RibbonLabel CarbonStorage;
    private RibbonButton carbonStorageOfTreesByStrataChartMenuItem;
    private RibbonButton carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem;
    private RibbonLabel AnnualCarbonSequestrationOfTreesLabel;
    private RibbonButton annualCarbonSequestrationOfTreesByStrataChartMenuItem;
    private RibbonButton annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem;
    private RibbonLabel EnergyEffectsLabel;
    private RibbonButton energyEffectsOfTreesMenuItem;
    private RibbonLabel HydrologyEffectsOfTrees;
    private RibbonButton HydrologyEffectsOfTreesBySpecies;
    private RibbonButton HydrologyEffectsOfTreesByStratum;
    private RibbonLabel OxygenProductionOfTreesLabel;
    private RibbonButton oxygenProductionOfTreesByStrataChartMenuItem;
    private RibbonButton oxygenProductionPerUnitAreaByStrataChartMenuItem;
    private RibbonLabel PollutionRemovalByTreesandShrubs;
    private RibbonButton monthlyPollutantRemovalByTreesAndShrubsMenuItem;
    private RibbonButton monthlyPollutantRemovalByTreesAndShrubsChartMenuItem;
    private RibbonButton hourlyPollutantRemovalByTreesAndShrubsChartMenuItem;
    private RibbonLabel BioemissionsOfTreesLabel;
    private RibbonButton bioemissionsOfTreesBySpeciesMenuItem;
    private RibbonButton bioemissionsOfTreesByStrataMenuItem;
    private RibbonMenu rmCompositionAndStructure;
    private RibbonLabel NumberOfTreesLabel;
    private RibbonButton numberOfTreesByStrataChartMenuItem;
    private RibbonButton numberOfTreesPerUnitAreaByStrataChartMenuItem;
    private RibbonLabel SpeciesCompositionLabel;
    private RibbonButton speciesCompositionByDBHClassAndStrataVerticalMenuItem;
    private RibbonButton speciesCompositionByDBHClassVerticalMenuItem;
    private RibbonLabel MostImportantTreeSpeciesLabel;
    private RibbonButton mostImportantTreeSpeciesMenuItem;
    private RibbonLabel SpeciesRichness;
    private RibbonButton speciesRichnessShannonWienerDiversityIndexMenuItem;
    private RibbonLabel OriginOfTreesByStrata;
    private RibbonButton originOfTreesByStrataMenuItem;
    private RibbonLabel ConditionOfTreesLabel;
    private RibbonButton conditionOfTreesBySpeciesMenuItem;
    private RibbonButton conditionOfTreesByStrataMenuItem;
    private RibbonLabel LeafAreaOfTrees;
    private RibbonButton leafAreaOfTreesByStrataChartMenuItem;
    private RibbonButton leafAreaOfTreesPerUnitAreaByStrataChartMenuItem;
    private RibbonLabel LeafAreaAndBiomassLabel;
    private RibbonButton leafAreaAndBiomassOfShrubsByStrataMenuItem;
    private RibbonButton leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem;
    private RibbonLabel GroundCoverCompositionLabel;
    private RibbonButton groundCoverCompositionByStrataMenuItem;
    private RibbonLabel LandUseCompositionLabel;
    private RibbonButton landUseCompositionByStrataMenuItem;
    private RibbonMenu rmIndividualLevelResults;
    private RibbonButton rbMDDCompositionOfTrees;
    private RibbonButton rbMDDTreeBenefitsPollutionRemoval;
    private RibbonButton rbMDDTreeBenefitsEnergyEffects;
    private RibbonButton rbMDDTreeBenefitsHydrologyEffects;
    private RibbonButton rbMDDTreeBenefitsOxygenProduction;
    private RibbonButton rbMDDTreeBenefitsVOCEmissions;
    private RibbonMenu rmPestAnalysis;
    private RibbonLabel PrimaryPestLabel;
    private RibbonButton primaryPestSummaryOfTreesByStrataButton;
    private RibbonButton primaryPestDetailsOfTreesByStrataButton;
    private RibbonLabel SignAndSymptomLabel;
    private RibbonButton signSymptomOverviewBySpeciesButton;
    private RibbonButton signSymptomOverviewByStrataButton;
    private RibbonButton signSymptomDetailsSummaryBySpeciesButton;
    private RibbonButton signSymptomDetailsCompleteBySpeciesButton;
    private RibbonButton signSymptomDetailsSummaryByStrataButton;
    private RibbonButton signSymptomDetailsCompleteByStrataButton;
    private RibbonButton signSymptomReviewOfTreesButton;
    private RibbonLabel PestReviewLabel;
    private RibbonButton pestReviewOfTreesButton;
    private RibbonTab rtData;
    private RibbonButton rbRetrieveMobileData;
    private RibbonTab rtProject;
    private RibbonGroup rgDataFields;
    private RibbonButton rbtnLandUses;
    private RibbonButton rbtnGroundCovers;
    private RibbonButton rbtnStreets;
    private RibbonButton rbtnLocSites;
    private RibbonButton rbtnBenefitPrices;
    private RibbonGroup rgDefinePlots;
    private RibbonButton definePlotsViaGoogleMaps;
    private RibbonConfigToolBar configToolBar;
    private RibbonMenu colorSchemeMenu;
    private RibbonMenu themeColorMenu;
    private RibbonToggleButton azureButton;
    private RibbonToggleButton blueButton;
    private RibbonToggleButton greenButton;
    private RibbonToggleButton orangeButton;
    private RibbonToggleButton orchidButton;
    private RibbonToggleButton redButton;
    private RibbonToggleButton tealButton;
    private RibbonToggleButton violetButton;
    private RibbonMenu themeLightnessMenu;
    private RibbonToggleButton darkGrayButton;
    private RibbonToggleButton lightGrayButton;
    private RibbonToggleButton whiteButton;
    private RibbonSeparator ribbonSeparator7;
    private RibbonToggleButton customButton;
    private RibbonToggleButton blue2007Button;
    private RibbonToggleButton silver2007Button;
    private RibbonToggleButton black2007Button;
    private RibbonToggleButton blue2010Button;
    private RibbonToggleButton silver2010Button;
    private RibbonToggleButton black2010Button;
    private RibbonToggleButton windows7Button;
    private RibbonButton minimizeRibbonButton;
    private RibbonButton expandRibbonButton;
    private RibbonButton helpConfigButton;
    private RibbonApplicationMenu appMenu;
    private RibbonButton exitButton;
    private RibbonButton mnuFileNewProject;
    private RibbonButton mnuFileOpenProject;
    private RibbonButton mnuFileOpenSampleProject;
    private RibbonButton mnuFileSaveProject;
    private RibbonButton mnuFileSaveProjectAs;
    private RibbonButton mnuFilePackProject;
    private RibbonButton mnuFilePrint;
    private RibbonButton mnuFileExport;
    private RibbonButton mnuFileImport;
    private RibbonButton mnuFileCloseProject;
    private RibbonListItem rmRecentProjects;
    private RibbonLabel rlProjectsLabel;
    private RibbonListItem rliSeparartor;
    private RibbonSplitButton undoSplitButton;
    private RibbonQat qat;
    private RibbonGroup rgDataActions;
    private RibbonButton rbDataNew;
    private RibbonButton rbDataCopy;
    private RibbonButton rbDataUndo;
    private RibbonButton rbDataDelete;
    private RibbonGroup rgPlot;
    private RibbonButton rbtnPlotLandUses;
    private RibbonButton rbtnPlotGroundCovers;
    private RibbonButton rbtnPlotRefObjects;
    private RibbonButton rbtnPlotTrees;
    private RibbonButton rbtnPlotShrubs;
    private RibbonButton rbtnPlotPlantingSites;
    private RibbonTab rtView;
    private RibbonGroup ribbonGroup8;
    private RibbonGroup rgInventoryData;
    private RibbonButton rbDataPlots;
    private RibbonButton rbDataTrees;
    private RibbonButton rbDataShrubs;
    private RibbonButton rbDataPlantingSites;
    private RibbonGroup BasicOptionsGroup;
    private RibbonToolBar ribbonToolBar1;
    private RibbonToolBar ribbonToolBar2;
    private RibbonButton rbDataRedo;
    private RibbonButton videoLearningButton;
    private RibbonButton rbPlantingSiteTypes;
    private RibbonButton definePlotsManuallyButton;
    private RibbonButton loadPlotsFromFileButton;
    private RibbonButton rbProjectExportCSV;
    private RibbonGroup rgUnits;
    private RibbonToggleButton rbEnglish;
    private RibbonToggleButton rbMetric;
    private RibbonButton BasicInputsButton;
    private RibbonGroup CustomOptionsGroup;
    private RibbonButton MortalityButton;
    private RibbonButton ReplantingButton;
    private RibbonGroup ConfigurationsGroup;
    private RibbonButton DefaultsButton;
    private RibbonButton NewForecastButton;
    private RibbonButton CopyForecastButton;
    private RibbonButton DeleteForecastButton;
    private RibbonComboBox ConfigurationComboBox;
    private RibbonGroup ForecastGroup;
    private RibbonButton RunForecastButton;
    public RibbonGroup rgForecastReports;
    private RibbonButton ForecastRenameButton;
    private RibbonButton rbtnOverview;
    private RibbonButton projectMetadataButton;
    private RibbonButton writtenReportButton;
    private RibbonGroup rgSpeciesName;
    private RibbonToggleButton rbSpeciesCN;
    private RibbonToggleButton rbSpeciesSN;
    private RibbonButton mnuFileCreateReinventory;
    private RibbonGroup dataEntryGroup;
    private RibbonComboBox rcboProject;
    private RibbonComboBox rcboSeries;
    private RibbonComboBox rcboYear;
    private RibbonMenu StructuralSplitButton;
    private RibbonButton NumTreesTotal;
    private RibbonButton PctCoverTotal;
    private RibbonButton DbhGrowthTotal;
    private RibbonButton DbhDistribTotal;
    private RibbonButton LeafAreaTotal;
    private RibbonButton TreeBiomassTotal;
    private RibbonButton TotalLfBiomassTotal;
    private RibbonButton AreaLfBiomassTotal;
    private RibbonButton BasalAreaTotal;
    private RibbonMenu FunctionalSplitButton;
    private RibbonButton ForecastPollutantSummary;
    private RibbonButton ForecastPollutantCO;
    private RibbonButton ForecastPollutantNO2;
    private RibbonButton ForecastPollutantO3;
    private RibbonButton ForecastPollutantSO2;
    private RibbonButton ForecastPollutantPM25;
    private RibbonButton ForecastPollutantPM10;
    private RibbonGroup rgSpecies;
    private RibbonGroup rgReportTree;
    private RibbonToggleButton ReportTreeViewToggle;
    private RibbonGroup rgProjectEditData;
    private RibbonButton rbProjectEnableEdit;
    private RibbonGroup rgProjectActions;
    private RibbonButton rbProjectNew;
    private RibbonButton rbProjectCopy;
    private RibbonButton rbProjectUndo;
    private RibbonButton rbProjectRedo;
    private RibbonButton rbProjectDelete;
    private RibbonButton rbtnPaperForm1;
    private RibbonGroup rgDataCollection;
    private RibbonButton rbSubmitMobileData;
    private RibbonMenu rmMaintenance;
    private RibbonButton rbtnMaintenanceRecommended;
    private RibbonButton rbtnMaintenanceTasks;
    private RibbonButton rbtnSidewalkConflicts;
    private RibbonButton rbtnUtilityConflicts;
    private RibbonMenu rmOther;
    private RibbonButton rbtnFieldOne;
    private RibbonButton rbtnFieldTwo;
    private RibbonButton rbtnFieldThree;
    private RibbonGroup rgSettings;
    private RibbonToggleButton rbReportsEnglish;
    private RibbonToggleButton rbReportsMetric;
    private RibbonToggleButton rbReportsSpeciesCN;
    private RibbonToggleButton rbReportsSpeciesSN;
    private RibbonLabel ribbonLabel1;
    private RibbonTab rtForecast;
    private RibbonGroup rgConfiguration;
    private RibbonCheckBox rcbAutoCheckUpdates;
    private RibbonGroup rgDataExport;
    private RibbonButton rbDataExportCSV;
    private RibbonButton rbDBH;
    private RibbonButton rbCondition;
    private RibbonMenu rmWeatherReports;
    private RibbonButton rbWeatherAirPollutantConcentrationUGM3;
    private RibbonButton rbWeatherPhotosyntheticallyActiveRadiation;
    private RibbonButton rbWeatherRain;
    private RibbonButton rbWeatherTempF;
    private RibbonButton rbWeatherAirQualImprovementTree;
    private RibbonButton rbWeatherAirQualImprovementShrub;
    private RibbonButton rbWeatherDryDepTree;
    private RibbonButton rbWeatherDryDepShrub;
    private RibbonButton rbTranspirationByTree;
    private RibbonButton rbTranspirationByShrub;
    private RibbonGroup rgProject;
    private RibbonGroup rgStrata;
    private RibbonGroup rgProjectExport;
    private RibbonButton rbMDDCompositionOfTreeBySpecies;
    private RibbonButton rbtnSpeciesCode;
    private RibbonGroup ribbonGroup12;
    private RibbonButton rbtnShowWhatsNew;
    private RibbonButton ParametersReportButton;
    private RibbonGroup ForecastLockGroup;
    private RibbonButton ForecastEditDataButton;
    private RibbonButton rbDataExportKML;
    private RibbonGroup rgDataMode;
    private RibbonButton rbDataEnableEdit;
    private RibbonButton rbProjectDefaults;
    private RibbonButton speciesCompositionByDBHClassAndStrataHorizontalMenuItem;
    private RibbonButton speciesCompositionByDBHClassHorizontalMenuItem;
    private RibbonMenu ExtremeEventsSplitButton;
    private RibbonButton PestsItem;
    private RibbonButton StormItem;
    private RibbonButton feedbackButton;
    private RibbonButton NumTreesByStrata;
    private RibbonButton PctCoverByStrata;
    private RibbonButton DbhGrowthByStrata;
    private RibbonButton LeafAreaByStrata;
    private RibbonButton TreeBiomassByStrata;
    private RibbonButton TotalLfBiomassByStrata;
    private RibbonButton AreaLfBiomassByStrata;
    private RibbonButton BasalAreaByStrata;
    private RibbonLabel ForecastCarbonLabel;
    private RibbonLabel ForecastPollutantLabel;
    private RibbonButton rmbModelProcessingNotes;
    private RibbonGroup rgModelNotes;
    private RibbonLabel RelativePerformanceIndexLabel;
    private RibbonButton rbRelativePerformanceIndexBySpecies;
    private RibbonLabel MaintenanceLabel;
    private RibbonButton rbMaintRecReport;
    private RibbonButton rbMaintTaskReport;
    private RibbonButton rbFieldOneReport;
    private RibbonButton rbFieldTwoReport;
    private RibbonButton rbFieldThreeReport;
    private RibbonButton numberOfTreesBySpecies;
    private RibbonButton speciesDistributionByDBHClassChart;
    private RibbonLabel ReplacementValueLabel;
    private RibbonButton ReplacementValueByDBHClassAndSpecies;
    private RibbonLabel ManagementCostsLabel;
    private RibbonButton rbManagementCostsByExpenditure;
    private RibbonLabel NetAnnualBenefitsLabel;
    private RibbonButton rbNetAnnualBenefitsForAllTreesSample;
    private RibbonLabel rlMDDCompositionAndStructure;
    private RibbonButton rbMDDCompositionOfTreesByStratum;
    private RibbonLabel rlMDDTreeBenefitsAndCosts;
    private RibbonButton rbMDDTreeBenefitsCarbonStorage;
    private RibbonButton rbMDDTreeBenefitsCarbonSequestration;
    private RibbonLabel SusceptibilityOfTreesToPestsLabel;
    private RibbonButton SusceptibilityOfTreesToPestsByStrata;
    private RibbonButton rbSidewalkConflictsReport;
    private RibbonButton rbUtilityConflictsReport;
    private RibbonLabel ForecastPopulationSummaryLabel;
    private RibbonLabel ForecastLeafAreaAndBiomassLabel;
    private RibbonButton ForecastCarbonStorage;
    private RibbonButton ForecastCarbonSequestration;
    private RibbonButton rbAnnualCosts;
    private RibbonButton rbPublicPrivateByStrata;
    private RibbonButton ForecastCarbonStorageByStrata;
    private RibbonButton ForecastCarbonSequestrationByStrata;
    private RibbonButton rbStreetTreesByStrata;
    private RibbonGroup rgInventoryValue;
    private RibbonButton SendDataButton;
    private RibbonButton RetrieveResultsButton;
    private RibbonButton rbValidate;
    private RibbonButton rbEula;
    private RibbonLabel OtherLabel;
    private RibbonButton rbImport;
    private RibbonLabel AirQualityImprovementLabel;
    private RibbonLabel AirPollutantFluxDryDepositionLabel;
    private RibbonLabel TranspirationLabel;
    private RibbonLabel lblStructureSummary;
    private RibbonButton rbStructureSummaryBySpecies;
    private RibbonButton rbStructureSummaryByStrataSpecies;
    private RibbonLabel BenefitsSummaryLabel;
    private RibbonButton rbBenefitsSummarySpecies;
    private RibbonButton rbBenefitsSummaryStrataSpecies;
    private C1.Win.C1Ribbon.C1Ribbon c1Ribbon1;
    public RibbonGroup rgReports;
    public RibbonGroup rgCharts;
    private RibbonMenu rmUserGuides;
    private RibbonButton rbUserGuidesInstall;
    private RibbonButton rbUserGuidesDataLimitations;
    private RibbonButton rbUserGuidesUnstratified;
    private RibbonButton rbUserGuidesPrestratified;
    private RibbonButton rbUserGuidesPoststratified;
    private RibbonButton rbUserGuidesUsingForecast;
    private RibbonSeparator ribbonSeparator2;
    private RibbonButton rbUserGuidesFieldManual;
    private RibbonButton rbUserGuidesExampleProjects;
    private RibbonButton rbUserGuidesInternational;
    private RibbonButton rbUserGuidesStratifyInventory;
    private RibbonSeparator rsImport;
    private RibbonCheckBox rcbMinimizeHelp;
    private RibbonGroup rgViewExport;
    private RibbonButton rbSpeciesList;
    private RibbonButton rbViewExportCSV;
    private RibbonButton LeafAreaIndexTotal;
    private RibbonButton LeafAreaIndexByStrata;
    private RibbonButton TreeCoverAreaTotal;
    private RibbonButton TreeCoverAreaByStrata;
    private RibbonButton annualCarbonSequestrationOfTreesBySpeciesChartMenuItem;
    private RibbonButton carbonStorageOfTreesBySpeciesChartMenuItem;
    private RibbonLabel AnnualNetCarbonSequestrationOfTreesLabel;
    private RibbonButton AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem;
    private RibbonButton AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem;
    private RibbonButton AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem;
    private RibbonButton rbUserGuidesDifferences_v5_v6;
    private RibbonSeparator ribbonSeparator3;
    private RibbonButton rbUserGuidesInventoryImporter;
    private RibbonGroup rgForecastUnits;
    private RibbonToggleButton rtbForecastEnglish;
    private RibbonToggleButton rtbForecastMetric;
    public RibbonButton rbStrata;
    private RibbonSeparator rsRecentProjects;
    private RibbonGroup rgExport;
    private RibbonButton rbReportExport;
    private RibbonButton rbMDDTreeBenefitsSummary;
    private RibbonLabel rlEvaporation;
    private RibbonButton rbEvaporationByTrees;
    private RibbonButton rbEvaporationByShrubs;
    private RibbonLabel rlWaterInterception;
    private RibbonButton rbWaterInterceptionByTrees;
    private RibbonButton rbWaterInterceptionByShrubs;
    private RibbonLabel rlAvoidedRunoff;
    private RibbonButton rbAvoidedRunoffByTrees;
    private RibbonButton rbAvoidedRunoffByShrubs;
    private RibbonLabel rbPotentialEvapotranspiration;
    private RibbonButton rbPotentialEvapotranspirationByTrees;
    private RibbonButton rbPotentialEvapotranspirationByShrubs;
    private RibbonLabel UVEffectsOfTreesLabel;
    private RibbonLabel WildLifeSuitabilityLabel;
    private RibbonButton UVEffectsOfTreesByStrata;
    private RibbonButton WildLifeSuitabilityByPlots;
    private RibbonButton WildLifeSuitabilityByStrata;
    private RibbonButton rbUVIndex;
    private RibbonLabel rbUVIndexReductionLabel;
    private RibbonButton rbUVIndexReductionByTreesForUnderTrees;
    private RibbonButton rbUVIndexReductionByTreesForOverall;
    private RibbonLabel rRawDataLabel;
    private RibbonButton rbDieback;
    private RibbonLabel FoodscapeBenefits;
    private RibbonButton FoodscapeBenefitsofTreesMenuItem;
    private RibbonLabel PollutionRemovalByGrass;
    private RibbonButton monthlyPollutantRemovalByGrassMenuItem;
    private RibbonButton monthlyPollutantRemovalByGrassChartMenuItem;
    private RibbonButton rbWeatherAirQualImprovementGrass;
    private RibbonButton rbWeatherDryDepGrass;
    private RibbonMenu rmAirQualityHealthImpactsAndValuesReports;
    private RibbonButton airQualityHealthImpactsandValuesbyTrees;
    private RibbonButton airQualityHealthImpactsandValuesbyShrubs;
    private RibbonButton airQualityHealthImpactsandValuesbyGrass;
    private RibbonButton airQualityHealthImpactsandValuesCombined;
    private RibbonLabel rlMDDMiscellaneous;
    private RibbonButton rbMDDMiscTreeComments;
    private RibbonButton rbMDDMiscShrubComments;
    private RibbonButton rbMDDMiscPlotComments;
    private RibbonCheckBox rbCheckBoxGPS;
    private RibbonCheckBox rbCheckBoxComments;
    private RibbonGroup rgMapping;
    private RibbonButton rbMapData;
    private RibbonButton rbUserGuidesCapturingCoordinateData;
    private RibbonCheckBox rbCheckBoxUID;
    private RibbonButton rbMaintTaskBySpecies;
    private RibbonButton rbMaintRecBySpecies;
    private RibbonButton rbSidewalkConflictsBySpecies;
    private RibbonButton rbUtilityConflictsBySpecies;
    private RibbonButton rbFieldOneBySpecies;
    private RibbonButton rbFieldTwoBySpecies;
    private RibbonButton rbFieldThreeBySpecies;
    private RibbonButton rbMDDCompositionOfPlots;
    private RibbonButton rbMaintRec;
    private RibbonLabel rbIsoprene;
    private RibbonButton rbIsopreneByTrees;
    private RibbonButton rbMonoterpeneByTrees;
    private RibbonButton rbIsopreneByShrubs;
    private RibbonButton rbMonoterpeneByShrubs;
    private RibbonLabel rbMonoterpene;
    private RibbonButton rbMaintTask;
    private RibbonButton rbSidewalkConflicts;
    private RibbonButton rbMaintUtilityConflicts;
    private RibbonButton rbFieldOne;
    private RibbonButton rbFieldTwo;
    private RibbonButton rbFieldThree;
    private RibbonSeparator ribbonSeparator4;
    private RibbonSeparator ribbonSeparator5;
    private RibbonGroup rgReportClasses;
    private RibbonButton rbRptDBH;
    private RibbonButton rbHealthClasses;
    private RibbonButton conditionOfTreesBySpeciesCustomMenuItem;
    private RibbonLabel CrownHealthLabel;
    private RibbonButton conditionOfTreesByStratumSpeciesCustomMenuItem;
    private RibbonButton rbDataDefaults;
    private RibbonButton rbtnShowChangeLog;
    private RibbonButton rbReportsExportKML;
    private RibbonButton rbReportsExportCSV;
    private RibbonButton rbMDDTreeEnergyAvoidedPollutants;
    private RibbonButton rbiTreeMethods;
    private RibbonButton rbDataReferenceObjects;
    private RibbonButton rbDataGroundCovers;
    private RibbonLabel LeafNutrients;
    private RibbonButton LeafNutrientsofTreesMenuItem;
    private RibbonButton rbMDDWoodProducts;
    private RibbonLabel AllergyIndexOfTreesLabel;
    private RibbonButton AllergyIndexOfTreesByStratum;
    private RibbonButton airQualityHealthImpactsandValuesbyTreesandShrubs;
    private RibbonButton rbDataLandUses;
    private RibbonCheckBox rbCheckBoxHideZeros;

    private void visualStyle_PressedButtonChanged(object sender, EventArgs e)
    {
      if (this.blue2007Button.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Office2007Blue;
      else if (this.silver2007Button.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Office2007Silver;
      else if (this.black2007Button.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Office2007Black;
      else if (this.blue2010Button.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Office2010Blue;
      else if (this.silver2010Button.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Office2010Silver;
      else if (this.black2010Button.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Office2010Black;
      else if (this.windows7Button.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Windows7;
      else if (this.customButton.Pressed)
        this.c1Ribbon1.VisualStyle = VisualStyle.Custom;
      this.m_ps.VisualStyle = this.c1Ribbon1.VisualStyle;
    }

    private void c1Ribbon1_VisualStyleChanged(object sender, EventArgs e)
    {
      int num = (int) (this.c1Ribbon1.VisualStyle - 4);
    }

    private void themeColor_PressedButtonChanged(object sender, EventArgs e)
    {
    }

    private void themeLightness_PressedButtonChanged(object sender, EventArgs e)
    {
    }

    internal bool WindowSwitching => this._windowSwitching;

    private void RefreshMdiWindowList(Form parentForm, RibbonMenu menu)
    {
      RibbonItemCollection items = menu.Items;
      items.ClearAndDisposeItems();
      Form[] mdiChildren = parentForm.MdiChildren;
      Form activeMdiChild = parentForm.ActiveMdiChild;
      int num = 1;
      for (int index = 0; index < mdiChildren.Length; ++index)
      {
        Form form = mdiChildren[index];
        if (form != null && !form.Disposing && !form.IsDisposed)
        {
          RibbonToggleButton ribbonToggleButton = new RibbonToggleButton();
          ribbonToggleButton.Click += new EventHandler(this.activateWindow_Click);
          ribbonToggleButton.CanDepress = false;
          ribbonToggleButton.Pressed = form == activeMdiChild;
          string str = form.Text;
          if (num < 10)
          {
            str = "&" + num.ToString() + " " + str;
            ++num;
          }
          ribbonToggleButton.Text = str;
          ribbonToggleButton.Tag = (object) form;
          ribbonToggleButton.ToggleGroupName = "MdiChildren";
          items.Add((RibbonItem) ribbonToggleButton);
        }
      }
      if (items.Count > 0)
      {
        menu.Enabled = true;
      }
      else
      {
        menu.Enabled = false;
        this._turnOffMdi = true;
        this.Invalidate();
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (!this._turnOffMdi)
        return;
      this.IsMdiContainer = false;
      this._turnOffMdi = false;
    }

    private void activateWindow_Click(object sender, EventArgs e)
    {
      RibbonToggleButton ribbonToggleButton = sender as RibbonToggleButton;
      if (!((RibbonItem) ribbonToggleButton != (RibbonItem) null))
        return;
      this._windowSwitching = true;
      Form tag = (Form) ribbonToggleButton.Tag;
      tag.Activate();
      if (tag.WindowState == FormWindowState.Minimized)
        tag.WindowState = FormWindowState.Normal;
      this._windowSwitching = false;
    }

    public MainRibbonForm(string[] args)
    {
      this.m_syncObj = new object();
      this.m_args = args;
      try
      {
        this.Init();
      }
      catch (Exception ex)
      {
        if (ex is VersionException && RTFMessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.UpdateRequired, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
          Program.LaunchUpdater();
        this.Shown += (EventHandler) ((s, e) => this.Close());
        return;
      }
      this.InitializeComponent();
      this.Text = string.Format("i-Tree Eco v{0}", (object) new Version(Application.ProductVersion).ToString(3));
      this.m_taskManager = new TaskManager(new WaitCursor((Form) this));
      this._locationService = new LocationService(this.m_ps.LocSp);
      this.RefreshMRUList();
      for (int index = 0; index < this.rmPestAnalysis.Items.Count; ++index)
      {
        RibbonButton ribbonButton = this.rmPestAnalysis.Items[index] as RibbonButton;
        if ((RibbonItem) ribbonButton != (RibbonItem) null)
          this.rmPestAnalysis.Items[index].Tag = (object) ribbonButton.Text;
      }
      this.rbEnglish.OwnerDraw = true;
      this.rbMetric.OwnerDraw = true;
      this.rbSpeciesCN.OwnerDraw = true;
      this.rbSpeciesSN.OwnerDraw = true;
      this.rbReportsEnglish.OwnerDraw = true;
      this.rbReportsMetric.OwnerDraw = true;
      this.rbReportsSpeciesCN.OwnerDraw = true;
      this.rbReportsSpeciesSN.OwnerDraw = true;
      this.rtbForecastEnglish.OwnerDraw = true;
      this.rtbForecastMetric.OwnerDraw = true;
      this.rgPlot.Visible = false;
      this.rgReportTree.Visible = false;
      this.runModelsButton.Visible = false;
      this.rgSpeciesName.Visible = false;
      this.rgUnits.Visible = false;
      this.rbPlantingSiteTypes.Visible = false;
      this.rbDataPlantingSites.Visible = false;
      this.rbtnLocSites.Visible = false;
      this.rbtnStreets.Visible = false;
      this.rbDataCopy.Visible = false;
      this.rbProjectCopy.Visible = false;
      this.rcbAutoCheckUpdates.Visible = false;
      this.rgExport.Visible = false;
      this.m_ps.InputSessionChanged += new EventHandler(this.InputSession_Changed);
      if (this.m_ps.UseEnglishUnits)
        this.rtbForecastEnglish.Pressed = true;
      else
        this.rtbForecastMetric.Pressed = true;
      if (this.m_ps.SpeciesDisplayName == SpeciesDisplayEnum.CommonName)
        this.rbSpeciesCN.Pressed = true;
      else
        this.rbSpeciesSN.Pressed = true;
      if (this.m_ps.MinimizeHelpPanel)
      {
        this.rcbMinimizeHelp.Checked = true;
        this.m_frmHelp.Hide();
      }
      EventPublisher.Register<EntityUpdated<Forecast>>(new EventHandler<EntityUpdated<Forecast>>(this.Forecast_Updated));
      EventPublisher.Register<EntityUpdated<Mortality>>(new EventHandler<EntityUpdated<Mortality>>(this.Mortality_Updated));
      EventPublisher.Register<EntityUpdated<Eco.Domain.v6.Project>>(new EventHandler<EntityUpdated<Eco.Domain.v6.Project>>(this.Project_Updated));
      EventPublisher.Register<EntityUpdated<Eco.Domain.v6.Series>>(new EventHandler<EntityUpdated<Eco.Domain.v6.Series>>(this.Series_Updated));
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      EventPublisher.Register<EntityCreated<Eco.Domain.v6.Plot>>(new EventHandler<EntityCreated<Eco.Domain.v6.Plot>>(this.Plot_Created));
      EventPublisher.Register<EntityDeleted<Eco.Domain.v6.Plot>>(new EventHandler<EntityDeleted<Eco.Domain.v6.Plot>>(this.Plot_Deleted));
    }

    private void InputSession_Changed(object sender, EventArgs e)
    {
      this.Text = string.Format("i-Tree Eco v{0}", (object) new Version(Application.ProductVersion).ToString(3));
      bool flag = this.m_ps.InputSession != null;
      this.mnuFileSaveProjectAs.Enabled = flag;
      this.mnuFilePackProject.Enabled = flag;
      this.mnuFileCloseProject.Enabled = flag;
      this.mnuFileCreateReinventory.Enabled = flag;
      if (flag)
      {
        lock (this.m_syncObj)
          this.m_session = this.m_ps.InputSession.CreateSession();
        this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
        this.m_ps.InputSession.ForecastChanged += new EventHandler(this.InputSession_ForcastChanged);
      }
      else
      {
        lock (this.m_syncObj)
          this.m_session = (ISession) null;
        staticData.ClearData();
        this.rtProject.Enabled = false;
        this.rtData.Enabled = false;
        this.rtReports.Enabled = false;
        this.rtForecast.Enabled = false;
        this.m_selectingProject = true;
        this.rcboProject.Items.Clear();
        this.rcboProject.Text = string.Empty;
        this.rcboSeries.Items.Clear();
        this.rcboSeries.Text = string.Empty;
        this.rcboYear.Items.Clear();
        this.rcboYear.Text = string.Empty;
        this.m_selectingProject = false;
        this.rbtnFieldOne.Text = v6Strings.FieldOne_SingularName;
        this.rbtnFieldTwo.Text = v6Strings.FieldTwo_SingularName;
        this.rbtnFieldThree.Text = v6Strings.FieldThree_SingularName;
        this.rbFieldOne.Text = v6Strings.FieldOne_SingularName;
        this.rbFieldTwo.Text = v6Strings.FieldTwo_SingularName;
        this.rbFieldThree.Text = v6Strings.FieldThree_SingularName;
        this.rbFieldOneBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldOne_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldTwoBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldTwo_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldThreeBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldThree_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldOneReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldOne_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
        this.rbFieldTwoReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldTwo_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
        this.rbFieldThreeReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldThree_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
        this.rtSupport.Selected = true;
        this.showTabSplash();
      }
    }

    private void Project_Updated(object sender, EntityUpdated<Eco.Domain.v6.Project> e)
    {
      if (this.m_session == null)
        return;
      Task.Factory.StartNew<Eco.Domain.v6.Project>((Func<Eco.Domain.v6.Project>) (() => this.m_session.Get<Eco.Domain.v6.Project>((object) e.Guid)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Eco.Domain.v6.Project>>) (t =>
      {
        if (this.IsDisposed)
          return;
        Eco.Domain.v6.Project p = t.Result;
        if (p == null)
          return;
        this._locationValid = RetryExecutionHandler.Execute<LocationSpecies.Domain.Location>((Func<LocationSpecies.Domain.Location>) (() =>
        {
          using (ISession session = this.m_ps.LocSp.OpenSession())
            return session.Get<LocationSpecies.Domain.Location>((object) p.LocationId);
        })) != null;
        foreach (RibbonButton ribbonButton in (CollectionBase) this.rcboProject.Items)
        {
          if (ribbonButton.Tag.Equals((object) p))
          {
            ribbonButton.Text = p.Name;
            if (!this.rcboProject.SelectedItem.Equals((object) ribbonButton))
              break;
            this.rcboProject.Text = ribbonButton.Text;
            break;
          }
        }
      }), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void Series_Updated(object sender, EntityUpdated<Eco.Domain.v6.Series> e)
    {
      if (this.m_session == null)
        return;
      Task.Factory.StartNew<Eco.Domain.v6.Series>((Func<Eco.Domain.v6.Series>) (() => this.m_session.Get<Eco.Domain.v6.Series>((object) e.Guid)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Eco.Domain.v6.Series>>) (t =>
      {
        if (this.IsDisposed)
          return;
        Eco.Domain.v6.Series result = t.Result;
        if (result == null)
          return;
        foreach (RibbonButton ribbonButton in (CollectionBase) this.rcboSeries.Items)
        {
          if (ribbonButton.Tag.Equals((object) result.Guid))
          {
            ribbonButton.Text = result.Id;
            if (this.rcboSeries.SelectedItem.Equals((object) ribbonButton))
            {
              this.rcboSeries.Text = ribbonButton.Text;
              break;
            }
            break;
          }
        }
        if (this.m_series == null || !(this.m_series.Guid == result.Guid))
          return;
        lock (this.m_syncObj)
          this.m_series = result;
        this.InitSeriesOptions();
      }), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void Plot_Created(object sender, EntityCreated<Eco.Domain.v6.Plot> e) => this.InitPlotOptions();

    private void Plot_Deleted(object sender, EntityDeleted<Eco.Domain.v6.Plot> e) => this.InitPlotOptions();

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_session == null)
        return;
      Task.Factory.StartNew<Year>((Func<Year>) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Evict((object) this.m_session.Load<Year>((object) e.Guid));
          Year year = this.m_session.Get<Year>((object) e.Guid);
          if (this.m_year != null && this.m_year.Guid == year.Guid)
          {
            NHibernateUtil.Initialize((object) year.YearLocationData);
            NHibernateUtil.Initialize((object) year.Results);
          }
          transaction.Commit();
          return year;
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Year>>) (t =>
      {
        if (this.IsDisposed)
          return;
        Year result = t.Result;
        if (result == null)
          return;
        foreach (RibbonButton ribbonButton in (CollectionBase) this.rcboYear.Items)
        {
          if (ribbonButton.Tag.Equals((object) result.Guid))
          {
            ribbonButton.Text = result.Id.ToString();
            if (this.rcboYear.SelectedItem.Equals((object) ribbonButton))
            {
              this.rcboYear.Text = ribbonButton.Text;
              break;
            }
            break;
          }
        }
        if (this.m_year == null || !(this.m_year.Guid == result.Guid))
          return;
        lock (this.m_syncObj)
          this.m_year = result;
        this.InitYearOptions();
        this.InitForecastOptions();
        this.InitPlotOptions();
      }), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void InputSession_YearChanged(object sender, EventArgs e)
    {
      bool hasValue = this.m_ps.InputSession.YearKey.HasValue;
      this.rgProject.Enabled = true;
      this.rtProject.Enabled = true;
      foreach (RibbonGroup group in (CollectionBase) this.rtProject.Groups)
      {
        if (group != this.rgProject && group != this.rgDefinePlots)
          group.Enabled = hasValue;
      }
      this.rtData.Enabled = hasValue;
      this.rtReports.Enabled = hasValue;
      this.rtForecast.Enabled = hasValue;
      if (!hasValue)
        return;
      Task.Factory.StartNew((System.Action) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          Year y = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey);
          NHibernateUtil.Initialize((object) y.YearLocationData);
          NHibernateUtil.Initialize((object) y.Series);
          NHibernateUtil.Initialize((object) y.Series.Project);
          NHibernateUtil.Initialize((object) y.Results);
          IList<Forecast> forecastList = this.m_session.QueryOver<Forecast>().Where((System.Linq.Expressions.Expression<Func<Forecast, bool>>) (f => f.Year == y)).List();
          try
          {
            transaction.Commit();
            lock (this.m_syncObj)
            {
              this.m_year = y;
              this.m_forecasts = forecastList;
            }
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (task =>
      {
        if (this.IsDisposed)
          return;
        if (this._locationValid & !this.m_year.Changed && this.m_ps.SpeciesVersion != (Version) null)
        {
          Version speciesVersion = this.m_year.SpeciesVersion;
          CultureInfo cultureInfo1 = this.m_year.Culture;
          CultureInfo cultureInfo2 = CultureInfo.CurrentCulture;
          CultureInfo cultureInfo3 = CultureInfo.GetCultureInfo("en");
          while (!cultureInfo2.IsNeutralCulture)
            cultureInfo2 = cultureInfo2.Parent;
          if (this.m_year.Culture == null)
          {
            if (!cultureInfo3.Equals((object) cultureInfo2) && !cultureInfo3.Equals((object) this.m_ps.SpeciesCulture))
            {
              int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.MsgReprocessSpecies, (object) cultureInfo3.DisplayName, (object) cultureInfo2.DisplayName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
          }
          else
          {
            while (!cultureInfo1.IsNeutralCulture)
              cultureInfo1 = cultureInfo1.Parent;
            bool flag = speciesVersion == (Version) null || speciesVersion < this.m_ps.SpeciesVersion;
            if (!cultureInfo1.Equals((object) this.m_ps.SpeciesCulture) | flag)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                Year year = this.m_year;
                try
                {
                  IList<string> stringList = this.m_session.GetNamedQuery("ListDistinctSpeciesClassifiers").SetParameter<Guid>("y", year.Guid).List<string>();
                  IQuery query = this.m_session.GetNamedQuery("UpdateSpeciesClassifierNames").SetParameter<Guid>("y", year.Guid);
                  foreach (string str in (IEnumerable<string>) stringList)
                  {
                    SpeciesView speciesView = (SpeciesView) null;
                    if (this.m_ps.Species.TryGetValue(str, out speciesView))
                      query.SetParameter<string>("cn", speciesView.CommonName).SetParameter<string>("sn", speciesView.ScientificName).SetParameter<string>("sp", str).ExecuteUpdate();
                  }
                  this.m_session.GetNamedQuery("UpdateIndividualTreeEffectsFromSpeciesClassifiers").SetParameter<Guid>("y", year.Guid).ExecuteUpdate();
                  this.m_session.GetNamedQuery("UpdateIndividualTreePollutionEffectsFromSpeciesClassifiers").SetParameter<Guid>("y", year.Guid).ExecuteUpdate();
                  year.Culture = this.m_ps.SpeciesCulture;
                  year.SpeciesVersion = this.m_ps.SpeciesVersion;
                  this.m_session.SaveOrUpdate((object) year);
                  transaction.Commit();
                  lock (this.m_syncObj)
                    this.m_year = year;
                }
                catch (HibernateException ex)
                {
                  transaction.Rollback();
                }
              }
            }
          }
        }
        this.InitYearOptions();
        this.InitForecasts();
        if (!this.m_showMetadataReport)
          return;
        this.c1Ribbon1.SelectedTab = this.rtReports;
        this.ShowDocument<ReportViewerForm>((object) this.projectMetadataButton).Report = (Report) new Report<ProjectMetadata>();
        this.m_showMetadataReport = false;
      }), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void InputSession_ForcastChanged(object sender, EventArgs e)
    {
      Guid? forecastKey = this.m_ps.InputSession.ForecastKey;
      if (!forecastKey.HasValue)
        return;
      this.LoadForecast(forecastKey.Value);
    }

    private void ContentForm_RequestRefresh(object sender, EventArgs e)
    {
      IDockContent activeDocument = this.dockPanel1.ActiveDocument;
      if (sender == null || !sender.Equals((object) activeDocument))
        return;
      switch (activeDocument)
      {
        case ProjectContentForm _:
          this.UpdateProjectActions();
          this.UpdateProjectExports();
          break;
        case DataContentForm _:
          this.UpdateDataActions();
          this.UpdateDataExports();
          break;
        case ViewContentForm _:
          this.UpdateViewExports();
          break;
        case ReportContentForm _:
          if (!(activeDocument is ReportViewerForm reportViewerForm))
            break;
          this.rbMapData.Enabled = reportViewerForm.Report.hasCoordinates;
          this.rbCheckBoxGPS.Enabled = reportViewerForm.Report.hasCoordinates;
          this.rbReportsExportKML.Enabled = reportViewerForm.Report.CanExport(ExportFormat.KML);
          this.rbReportsExportCSV.Enabled = reportViewerForm.Report.CanExport(ExportFormat.CSV);
          this.rbCheckBoxUID.Enabled = reportViewerForm.Report.hasUID;
          this.rbCheckBoxComments.Enabled = reportViewerForm.Report.hasComments;
          this.rbCheckBoxHideZeros.Enabled = reportViewerForm.Report.hasZeros;
          break;
      }
    }

    private void UpdateViewExports()
    {
      if (this.dockPanel1.ActiveDocument is IExportable activeDocument)
      {
        this.rgViewExport.Enabled = true;
        this.rbViewExportCSV.Enabled = activeDocument.CanExport(ExportFormat.CSV);
      }
      else
        this.rgViewExport.Enabled = false;
    }

    private void ContentForm_ShowHelp(object sender, ShowHelpEventArgs e) => this.ShowHelp(e.Topic);

    private void ProjectOverviewClosed(object sender, FormClosedEventArgs e)
    {
      if (sender is ProjectOverviewForm projectOverviewForm && projectOverviewForm.ProjectCreated)
      {
        this.AddToMRUList(this.m_ps.InputSession.InputDb);
        this.m_taskManager.Add(this.InitProjects());
      }
      else if (this.m_ps.InputSession == null || !this.m_ps.InputSession.YearKey.HasValue)
      {
        this.rtProject.Enabled = true;
        this.rgProject.Enabled = true;
        foreach (RibbonGroup group in (CollectionBase) this.rtProject.Groups)
        {
          if (group != this.rgProject)
            group.Enabled = false;
        }
        this.rtData.Enabled = false;
        this.rtReports.Enabled = false;
        this.rtForecast.Enabled = false;
      }
      this.c1Ribbon1.Enabled = true;
    }

    private void CloseSplashForm()
    {
      if (MainRibbonForm.splashForm == null)
        return;
      MainRibbonForm.splashForm.Invoke((Delegate) (() => MainRibbonForm.splashForm.CloseSplashForm()));
      MainRibbonForm.splashForm = (SplashForm) null;
    }

    private void ShowWhatsNew()
    {
      this.Activate();
      this.Opacity = 100.0;
      if (this.m_args != null && this.m_args.Length == 1)
        this.OpenProjectFile(this.m_args[0]);
      this.Activate();
      if (!Settings.Default.ShowWhatsNewOnLaunch)
        return;
      int num = (int) new WhatsNewForm().ShowDialog((IWin32Window) this);
    }

    private void MainRibbonForm_Load(object sender, EventArgs e)
    {
      this.c1Ribbon1.VisualStyle = this.m_ps.VisualStyle;
      switch (this.c1Ribbon1.VisualStyle)
      {
        case VisualStyle.Custom:
          this.customButton.Pressed = true;
          break;
        case VisualStyle.Office2007Black:
          this.black2007Button.Pressed = true;
          break;
        case VisualStyle.Office2007Blue:
          this.blue2007Button.Pressed = true;
          break;
        case VisualStyle.Office2007Silver:
          this.silver2007Button.Pressed = true;
          break;
        case VisualStyle.Office2010Black:
          this.black2010Button.Pressed = true;
          break;
        case VisualStyle.Office2010Blue:
          this.blue2010Button.Pressed = true;
          break;
        case VisualStyle.Office2010Silver:
          this.silver2010Button.Pressed = true;
          break;
        case VisualStyle.Windows7:
          this.windows7Button.Pressed = true;
          break;
      }
      this.customButton.Visible = false;
      this.themeColorMenu.Visible = false;
      this.themeLightnessMenu.Visible = false;
      this.m_showSplashTabSplash = true;
      this.c1Ribbon1.SelectedTab = this.rtSupport;
    }

    private void Init()
    {
      Thread thread = new Thread((ThreadStart) (() =>
      {
        MainRibbonForm.splashForm = new SplashForm();
        Application.Run((Form) MainRibbonForm.splashForm);
      }));
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
      DateTime now = DateTime.Now;
      this.m_ps = ProgramSession.GetInstance();
      TimeSpan timeSpan = DateTime.Now - now;
      int int32 = Convert.ToInt32(timeSpan.TotalMilliseconds);
      if (timeSpan.TotalMilliseconds < 5000.0)
        Thread.Sleep(5000 - int32);
      this.CloseSplashForm();
      this.Shown += (EventHandler) ((s, e) => this.ShowWhatsNew());
    }

    private void mnuFileNewProject_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate())
        return;
      NewProjectType newProjectType = new NewProjectType();
      if (newProjectType.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.CheckPathExists = true;
        saveFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterEcoFile, (object) string.Join(";", Settings.Default.ExtEcoProj)),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
        });
        saveFileDialog.DefaultExt = Regex.Replace(Settings.Default.ExtEcoProj[0], "^\\*?\\.", "");
        saveFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.SaveProjectAs;
        saveFileDialog.ShowHelp = false;
        saveFileDialog.CreatePrompt = false;
        saveFileDialog.OverwritePrompt = true;
        if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.CreateProject(sender, saveFileDialog.FileName, newProjectType.NewProjectSampleType);
      }
    }

    private void CreateProject(object sender, string ProjectPath, SampleType s)
    {
      this.c1Ribbon1.Enabled = false;
      if (this.CreateDatabase(ProjectPath))
      {
        ProgramSession session = Program.Session;
        if (session.InputSession != null)
          session.InputSession.Close();
        this.CloseAllDocuments();
        this.c1Ribbon1.SelectedTab = this.rtProject;
        session.InputSession = new InputSession(ProjectPath);
        if (this.ShowDocument<ProjectOverviewForm>(sender, (object) s) == null)
          return;
        this.ShowHelp("tpProjectInfo");
      }
      else
        this.c1Ribbon1.Enabled = true;
    }

    private void mnuFileCloseProject_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate())
        return;
      InputSession inputSession = this.m_ps.InputSession;
      if (inputSession != null)
      {
        inputSession.Close();
        this.m_ps.InputSession = (InputSession) null;
      }
      this.m_showSplashTabSplash = true;
      this.showTabSplash();
    }

    private void mnuFileOpenProject_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate())
        return;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.CheckPathExists = true;
      openFileDialog1.Multiselect = false;
      openFileDialog1.Filter = string.Join("|", new string[2]
      {
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterEcoFile, (object) string.Join(";", Settings.Default.ExtEcoProj)),
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
      });
      openFileDialog1.Title = i_Tree_Eco_v6.Resources.Strings.SelectProjectFile;
      openFileDialog1.ShowHelp = false;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (Settings.Default.SetInitialOpenDirectory)
      {
        openFileDialog2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        Settings.Default.SetInitialOpenDirectory = false;
      }
      if (openFileDialog2.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.OpenProjectFile(openFileDialog2.FileName);
    }

    private void OpenProjectFile(string fn, bool isSample = false)
    {
      bool flag1 = FileSignature.IsAccessDatabase(fn);
      bool flag2 = FileSignature.IsSqliteDatabase(fn);
      if (flag1 | flag2)
      {
        this.m_showMetadataReport = true;
        InputSession inputSession1 = new InputSession(fn);
        if (inputSession1.UpdateRequired() | flag1)
        {
          if (RTFMessageBox.Show((IWin32Window) this, ApplicationHelp.UpgradeV6Warning, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            return;
          using (TempFile tempFile = new TempFile(false))
          {
            File.Copy(fn, tempFile.name, true);
            using (InputSession inputSession2 = new InputSession(tempFile.name, true))
            {
              if (inputSession1.UpdateRequired() && new ProjectUpdateForm((UpdateDatabaseTask) new UpdateV6DatabaseTask(inputSession2)).ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              SaveFileDialog saveFileDialog1 = new SaveFileDialog();
              saveFileDialog1.Title = i_Tree_Eco_v6.Resources.Strings.SaveProjectAs;
              saveFileDialog1.Filter = string.Join("|", new string[2]
              {
                string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterEcoFile, (object) string.Join(";", Settings.Default.ExtEcoProj)),
                string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
              });
              saveFileDialog1.CheckPathExists = true;
              saveFileDialog1.OverwritePrompt = true;
              saveFileDialog1.AddExtension = true;
              saveFileDialog1.DefaultExt = Regex.Replace(Settings.Default.ExtEcoProj[0], "^\\*?\\.", "");
              saveFileDialog1.FileName = Path.GetFileName(fn);
              saveFileDialog1.ShowHelp = false;
              saveFileDialog1.CreatePrompt = false;
              SaveFileDialog saveFileDialog2 = saveFileDialog1;
              if (saveFileDialog2.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (flag1)
              {
                TaskScheduler.FromCurrentSynchronizationContext();
                if (new CreateProjectTask(saveFileDialog2.FileName).DoWork().GetAwaiter().GetResult())
                {
                  InputSession dst = new InputSession(saveFileDialog2.FileName);
                  CancellationTokenSource token = new CancellationTokenSource();
                  Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
                  CopyProjectTask copyProjectTask = new CopyProjectTask(inputSession2, dst, new CancellationToken?(token.Token), (IProgress<ProgressEventArgs>) progress);
                  ProcessingForm processingForm = new ProcessingForm();
                  processingForm.Text = "Upgrading project format..";
                  if (processingForm.ShowDialog((IWin32Window) this, copyProjectTask.DoWork(), progress, token) != DialogResult.OK)
                    return;
                }
              }
              else
                File.Copy(tempFile.name, saveFileDialog2.FileName, true);
              fn = saveFileDialog2.FileName;
              inputSession1 = new InputSession(fn);
            }
          }
          int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.MsgUpgradeV6Success, Application.ProductName, MessageBoxButtons.OK);
        }
        if (!inputSession1.CompatibleWithModel())
        {
          int num1 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrProjectIncompatible, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          this.LoadProject(fn, isSample);
          if (isSample)
            return;
          this.AddToMRUList(fn);
        }
      }
      else
        this.OpenV5Project(fn);
    }

    private void OpenV5Project(string fn)
    {
      if (RTFMessageBox.Show((IWin32Window) this, ApplicationHelp.UpgradeV5Warning, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      if (!(ProjectFile.Open(fn)["FieldInputDatabasePath"] is string str))
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrInvalidProjectFile, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!File.Exists(str))
        {
          str = Path.Combine(Path.GetDirectoryName(fn), Path.GetFileName(str));
          if (!File.Exists(str))
          {
            int num2 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrInputDBNotFound, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (!FileSignature.IsAccessDatabase(str))
        {
          int num3 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrInvalidDBFormat, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          using (TempFile tempFile = new TempFile(false, ".mdb"))
          {
            File.Copy(str, tempFile.name, true);
            if (new ProjectUpdateForm((UpdateDatabaseTask) new UpdateV5DatabaseTask(tempFile.name)).ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = i_Tree_Eco_v6.Resources.Strings.SaveProjectAs;
            saveFileDialog1.Filter = string.Join("|", new string[2]
            {
              string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterEcoFile, (object) string.Join(";", Settings.Default.ExtEcoProj)),
              string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
            });
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = Regex.Replace(Settings.Default.ExtEcoProj[0], "^\\*?\\.", "");
            saveFileDialog1.ShowHelp = false;
            saveFileDialog1.CreatePrompt = false;
            SaveFileDialog saveFileDialog2 = saveFileDialog1;
            if (saveFileDialog2.ShowDialog((IWin32Window) this) != DialogResult.OK || new ConvertProjectForm(tempFile.name, saveFileDialog2.FileName).ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            int num4 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.MsgUpgradeSuccess, Application.ProductName, MessageBoxButtons.OK);
            this.m_showMetadataReport = true;
            this.LoadProject(saveFileDialog2.FileName);
            this.AddToMRUList(saveFileDialog2.FileName);
          }
        }
      }
    }

    private void LoadProject(string fn) => this.LoadProject(fn, false);

    private void LoadProject(string fn, bool temporary)
    {
      if (!this.CloseAllDocuments())
        return;
      if (this.m_ps.InputSession != null)
        this.m_ps.InputSession.Close();
      this.m_ps.InputSession = new InputSession(fn, temporary);
      this.m_taskManager.Add(this.InitProjects());
    }

    private void AddToMRUList(string fn)
    {
      List<string> stringList = new List<string>();
      string[] mruList = this.m_ps.MRUList;
      stringList.Add(fn);
      if (mruList != null)
      {
        for (int index = 0; index < mruList.Length && stringList.Count < 10; ++index)
        {
          if (!fn.Equals(mruList[index], StringComparison.CurrentCultureIgnoreCase))
            stringList.Add(mruList[index]);
        }
      }
      this.m_ps.MRUList = stringList.ToArray();
      this.RefreshMRUList();
    }

    private void RefreshMRUList()
    {
      while (this.appMenu.RightPaneItems.Count > 2)
        this.appMenu.RightPaneItems.RemoveAt(this.appMenu.RightPaneItems.Count - 1);
      string[] mruList = this.m_ps.MRUList;
      if (mruList == null)
        return;
      foreach (string path in mruList)
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(path);
        RibbonButton ribbonButton = new RibbonButton();
        RibbonListItem ribbonListItem = new RibbonListItem((RibbonItem) ribbonButton);
        ribbonButton.Text = withoutExtension;
        ribbonListItem.ToolTip = path;
        ribbonListItem.Click += new EventHandler(this.rliMRUList_Click);
        this.appMenu.RightPaneItems.Add((RibbonItem) ribbonListItem);
      }
    }

    private void rliMRUList_Click(object sender, EventArgs e)
    {
      RibbonListItem ribbonListItem = sender as RibbonListItem;
      if (!((RibbonItem) ribbonListItem != (RibbonItem) null))
        return;
      string toolTip = ribbonListItem.ToolTip;
      if (!File.Exists(toolTip))
      {
        if (MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrProjectNotFound, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) this.m_ps.MRUList);
        stringList.Remove(toolTip);
        this.m_ps.MRUList = stringList.ToArray();
        this.RefreshMRUList();
      }
      else
      {
        if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate())
          return;
        this.OpenProjectFile(ribbonListItem.ToolTip);
      }
    }

    private void LoadForecast(Guid fKey)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      Task.Factory.StartNew<Forecast>((Func<Forecast>) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          try
          {
            Forecast forecast1 = this.m_session.Load<Forecast>((object) fKey);
            if (this.m_session.Contains((object) forecast1))
              this.m_session.Evict((object) forecast1);
            Forecast forecast2 = this.m_session.Get<Forecast>((object) fKey);
            if (forecast2 != null)
            {
              NHibernateUtil.Initialize((object) forecast2.Mortalities);
              NHibernateUtil.Initialize((object) forecast2.Replanting);
              NHibernateUtil.Initialize((object) forecast2.PestEvents);
              NHibernateUtil.Initialize((object) forecast2.WeatherEvents);
            }
            transaction.Commit();
            return forecast2;
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Forecast>>) (task =>
      {
        if (this.IsDisposed || task.IsFaulted)
          return;
        Forecast result = task.Result;
        lock (this.m_syncObj)
          this.m_forecast = result;
        this.InitForecastOptions();
      }), scheduler);
    }

    private void LoadFrostFreeDays(int locId)
    {
      GrowthPeriod growthPeriod = this._locationService.GetGrowthPeriod(locId);
      if (growthPeriod != null)
        this._ffDays = growthPeriod.FrostFreeDays;
      else
        this._ffDays = (short) 149;
    }

    private void c1Ribbon1_MinimizedChanged(object sender, EventArgs e)
    {
      if (this.c1Ribbon1.Minimized)
      {
        this.minimizeRibbonButton.Visible = false;
        this.expandRibbonButton.Visible = true;
      }
      else
      {
        this.minimizeRibbonButton.Visible = true;
        this.expandRibbonButton.Visible = false;
      }
    }

    private void minimizeRibbonButton_Click(object sender, EventArgs e) => this.c1Ribbon1.Minimized = true;

    private void expandRibbonButton_Click(object sender, EventArgs e) => this.c1Ribbon1.Minimized = false;

    private void rcListPinButton_Click(object sender, EventArgs e)
    {
      RibbonToggleButton ribbonToggleButton = (RibbonToggleButton) sender;
      if (ribbonToggleButton.Pressed)
        ribbonToggleButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.Pinned;
      else
        ribbonToggleButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.Unpinned;
    }

    private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
    {
      IDockContent activeDocument = this.dockPanel1.ActiveDocument;
      switch (activeDocument)
      {
        case DataContentForm _:
          this.UpdateDataActions();
          this.UpdateDataExports();
          break;
        case ProjectContentForm _:
          this.UpdateProjectActions();
          this.UpdateProjectExports();
          break;
        case ViewContentForm _:
          this.UpdateViewExports();
          break;
        default:
          this.rgDataActions.Visible = false;
          this.rgProjectActions.Visible = false;
          this.rbImport.Enabled = false;
          this.rgViewExport.Enabled = false;
          this.rgDataExport.Enabled = false;
          this.rgProjectExport.Enabled = false;
          if (activeDocument != null || this.m_closingDocuments)
            break;
          this.showTabSplash();
          break;
      }
    }

    private void c1Ribbon1_SelectedTabChanged(object sender, EventArgs e)
    {
      if (!this.c1Ribbon1.Enabled)
        return;
      if (!this.m_tabChangeCanceled)
        this.showTabSplash();
      this.m_tabChangeCanceled = false;
    }

    private void exitButton_Click(object sender, EventArgs e) => this.Close();

    private void MainRibbonForm_FormClosed(object sender, FormClosedEventArgs e) => this.m_ps.Save();

    private void SilentUpdate()
    {
      try
      {
        RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("Software\\i-Tree\\v3\\", false);
        if (registryKey == null)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrNoVersionFound, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string str1 = Path.Combine(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.ExecutablePath)), "UpdateTool"), "iTreeUpdater.exe");
          string[] strArray = new string[2]
          {
            registryKey.GetValue("Version", (object) string.Empty).ToString(),
            "true"
          };
          StringBuilder stringBuilder = new StringBuilder();
          foreach (string input in strArray)
          {
            string str2 = Regex.Replace(Regex.Replace(input, "(\\\\+)\"", "$1$1\"").Replace("\"", "\\\""), "(\\\\+)$", "$1$1");
            stringBuilder.Append(string.Format("\"{0}\"", (object) str2));
          }
          new Process()
          {
            StartInfo = {
              FileName = str1,
              Arguments = stringBuilder.ToString(),
              UseShellExecute = false
            }
          }.Start();
        }
      }
      catch (SecurityException ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrInsufficientPrivileges, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrUnexpected, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private bool CreateDatabase(string dbPath)
    {
      Task<bool> task = new CreateProjectTask(dbPath).DoWork();
      try
      {
        this.m_taskManager.Add((Task) task);
        task.Wait();
      }
      catch (AggregateException ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrCreateProject, (object) ex.InnerException.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      return task.Result;
    }

    private Task InitProjects()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew<IList<Eco.Domain.v6.Project>>((Func<IList<Eco.Domain.v6.Project>>) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          try
          {
            return this.m_session.CreateCriteria<Eco.Domain.v6.Project>().AddOrder(Order.Asc("Name")).List<Eco.Domain.v6.Project>();
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<IList<Eco.Domain.v6.Project>>>) (t =>
      {
        if (this.IsDisposed || t.IsFaulted)
          return;
        IList<Eco.Domain.v6.Project> result = t.Result;
        this.rcboProject.Items.Clear();
        if (result.Count == 0)
        {
          this.c1Ribbon1.SelectedTab = this.rtProject;
          NewProjectType newProjectType = new NewProjectType();
          if (newProjectType.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          {
            this.ProjectOverviewClosed((object) this, new FormClosedEventArgs(CloseReason.None));
          }
          else
          {
            this.Text = string.Format("i-Tree Eco v{0}", (object) new Version(Application.ProductVersion).ToString(3));
            if (this.ShowDocument<ProjectOverviewForm>((object) this.rbtnOverview, (object) newProjectType.NewProjectSampleType) != null)
              this.ShowHelp("tpProjectInfo");
            this.c1Ribbon1.Enabled = false;
          }
        }
        else
        {
          foreach (Eco.Domain.v6.Project project in (IEnumerable<Eco.Domain.v6.Project>) result)
          {
            RibbonItemCollection items = this.rcboProject.Items;
            items.Add((RibbonItem) new RibbonButton()
            {
              Text = project.Name,
              Tag = (object) project
            });
          }
          this.Text = string.Format("[{0}: {1}] - i-Tree Eco v{2}", (object) v6Strings.Project_SingularName, (object) this.rcboProject.Text, (object) new Version(Application.ProductVersion).ToString(3));
          this.rcboProject.SelectedIndex = 0;
          this.rcboProject.Enabled = true;
        }
      }), scheduler);
    }

    private Task InitSeries(Eco.Domain.v6.Project p)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew<IList<Eco.Domain.v6.Series>>((Func<IList<Eco.Domain.v6.Series>>) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          try
          {
            return this.m_session.CreateCriteria<Eco.Domain.v6.Series>().Add((ICriterion) Restrictions.Eq("Project", (object) p)).AddOrder(Order.Asc("Id")).List<Eco.Domain.v6.Series>();
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<IList<Eco.Domain.v6.Series>>>) (t =>
      {
        if (this.IsDisposed || t.IsFaulted)
          return;
        IList<Eco.Domain.v6.Series> result = t.Result;
        this.rcboSeries.Items.Clear();
        foreach (Eco.Domain.v6.Series series in (IEnumerable<Eco.Domain.v6.Series>) result)
        {
          RibbonItemCollection items = this.rcboSeries.Items;
          items.Add((RibbonItem) new RibbonButton()
          {
            Text = series.Id,
            Tag = (object) series
          });
        }
        this.rcboSeries.SelectedIndex = 0;
        this.rcboSeries.Enabled = true;
        this.Text = string.Format("[{0}: {1}] [{2}: {3}] - i-Tree Eco v{4}", (object) v6Strings.Project_SingularName, (object) this.rcboProject.Text, (object) v6Strings.Series_SingularName, (object) this.rcboSeries.Text, (object) new Version(Application.ProductVersion).ToString(3));
      }), scheduler);
    }

    private void InitSeriesOptions()
    {
      bool isSample = this.m_series.IsSample;
      this.rgDefinePlots.Visible = isSample;
      this.rbDataPlots.Visible = isSample;
      this.rbDataReferenceObjects.Visible = isSample;
      this.rbDataGroundCovers.Visible = isSample;
      this.rbDataLandUses.Visible = isSample;
      this.rmBenefitsAndCosts.Enabled = true;
      this.rmCompositionAndStructure.Enabled = true;
    }

    private void InitYearOptions()
    {
      Year y = this.m_year;
      if (y.RecordOtherOne && !string.IsNullOrEmpty(y.OtherOne))
      {
        this.rbtnFieldOne.Text = y.OtherOne;
        this.rbFieldOne.Text = y.OtherOne;
        this.rbFieldOneBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) y.OtherOne, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldOneReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) y.OtherOne, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
      }
      else
      {
        this.rbtnFieldOne.Text = v6Strings.FieldOne_SingularName;
        this.rbFieldOne.Text = v6Strings.FieldOne_SingularName;
        this.rbFieldOneBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldOne_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldOneReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldOne_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
      }
      if (y.RecordOtherTwo && !string.IsNullOrEmpty(y.OtherTwo))
      {
        this.rbtnFieldTwo.Text = y.OtherTwo;
        this.rbFieldTwo.Text = y.OtherTwo;
        this.rbFieldTwoBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) y.OtherTwo, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldTwoReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) y.OtherTwo, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
      }
      else
      {
        this.rbtnFieldTwo.Text = v6Strings.FieldTwo_SingularName;
        this.rbFieldTwo.Text = v6Strings.FieldTwo_SingularName;
        this.rbFieldTwoBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldTwo_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldTwoReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldTwo_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
      }
      if (y.RecordOtherThree && !string.IsNullOrEmpty(y.OtherThree))
      {
        this.rbtnFieldThree.Text = y.OtherThree;
        this.rbFieldThree.Text = y.OtherThree;
        this.rbFieldThreeBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) y.OtherThree, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldThreeReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) y.OtherThree, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
      }
      else
      {
        this.rbtnFieldThree.Text = v6Strings.FieldThree_SingularName;
        this.rbFieldThree.Text = v6Strings.FieldThree_SingularName;
        this.rbFieldThreeBySpecies.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldThree_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpecies);
        this.rbFieldThreeReport.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.FieldThree_SingularName, (object) i_Tree_Eco_v6.Resources.Strings.BySpeciesAndDBH);
      }
      ReportOptions reportOptions = new ReportOptions(y);
      foreach (RibbonGroup ribbonGroup in new List<RibbonGroup>()
      {
        this.rgReports,
        this.rgCharts,
        this.rgForecastReports
      })
      {
        Stack<RibbonItem> ribbonItemStack = new Stack<RibbonItem>();
        foreach (RibbonItem ribbonItem in (CollectionBase) ribbonGroup.Items)
          ribbonItemStack.Push(ribbonItem);
        while (ribbonItemStack.Count > 0)
        {
          RibbonItem ribbonItem1 = ribbonItemStack.Pop();
          ReportCondition condition = reportOptions.GetCondition(ribbonItem1.Name);
          if (condition != null)
            ribbonItem1.Visible = condition.IsAvailable && condition.Enabled;
          RibbonMenu ribbonMenu = ribbonItem1 as RibbonMenu;
          if ((RibbonItem) ribbonMenu != (RibbonItem) null)
          {
            foreach (RibbonItem ribbonItem2 in (CollectionBase) ribbonMenu.Items)
              ribbonItemStack.Push(ribbonItem2);
          }
        }
      }
      this.rbtnLandUses.Visible = y.RecordLanduse;
      this.rbtnGroundCovers.Visible = y.RecordGroundCover;
      this.rbDBH.Visible = !y.DBHActual;
      this.rbCondition.Visible = y.RecordCrownCondition && y.DisplayCondition;
      this.rbDieback.Visible = y.RecordCrownCondition && !y.DisplayCondition;
      this.rbtnStreets.Visible = this.m_series.SampleType == SampleType.StratumStreetTree && y.RecordPlotAddress;
      this.rbtnLocSites.Visible = y.RecordLocSite;
      this.rbtnMaintenanceRecommended.Visible = y.RecordMaintRec;
      this.rbtnMaintenanceTasks.Visible = y.RecordMaintTask;
      this.rbtnUtilityConflicts.Visible = y.RecordWireConflict;
      this.rbtnSidewalkConflicts.Visible = y.RecordSidewalk;
      this.rmMaintenance.Visible = y.RecordMaintRec || y.RecordMaintTask || y.RecordSidewalk || y.RecordWireConflict;
      this.rbtnFieldOne.Visible = y.RecordOtherOne;
      this.rbtnFieldTwo.Visible = y.RecordOtherTwo;
      this.rbtnFieldThree.Visible = y.RecordOtherThree;
      this.rmOther.Visible = y.RecordOtherOne || y.RecordOtherTwo || y.RecordOtherThree;
      this.rgDataFields.Visible = this.rbtnLandUses.Visible || this.rbtnGroundCovers.Visible || this.rbDBH.Visible || this.rbCondition.Visible || this.rbDieback.Visible || this.rbtnStreets.Visible || this.rbtnLocSites.Visible || this.rbPlantingSiteTypes.Visible || this.rmMaintenance.Visible || this.rmOther.Visible;
      bool flag = this._locationValid && y.RevProcessed > 0;
      this.SendDataButton.Enabled = this._locationValid;
      this.RetrieveResultsButton.Enabled = this._locationValid && !y.Changed && y.Results.Where<YearResult>((Func<YearResult, bool>) (r => r.RevProcessed == y.Results.Max<YearResult>((Func<YearResult, int>) (yr => yr.RevProcessed)) && !r.Completed)).Count<YearResult>() > 0;
      this.rgDefinePlots.Enabled = y.ConfigEnabled;
      this.rbStrata.Enabled = y.RecordStrata;
      this.rbRetrieveMobileData.Enabled = y.Changed && !string.IsNullOrEmpty(y.MobileKey);
      this.rgReports.Enabled = flag;
      this.rgCharts.Enabled = flag;
      this.rgModelNotes.Enabled = flag;
      this.ForecastGroup.Enabled = flag;
      this.rbProjectEnableEdit.Enabled = !y.ConfigEnabled;
      if (y.ConfigEnabled)
      {
        this.rbProjectEnableEdit.Text = i_Tree_Eco_v6.Resources.Strings.EditingEnabled;
        this.rbProjectEnableEdit.ToolTip = i_Tree_Eco_v6.Resources.Strings.ToolTipEditingEnabledConfigToolTip;
      }
      else
      {
        this.rbProjectEnableEdit.Text = i_Tree_Eco_v6.Resources.Strings.EditingDisabled;
        this.rbProjectEnableEdit.ToolTip = i_Tree_Eco_v6.Resources.Strings.ToolTipEditingDisabledConfig;
      }
      this.rbDataEnableEdit.Enabled = !y.Changed;
      if (y.Changed)
      {
        this.rbDataEnableEdit.Text = i_Tree_Eco_v6.Resources.Strings.EditingEnabled;
        this.rbDataEnableEdit.ToolTip = i_Tree_Eco_v6.Resources.Strings.ToolTipEditingEnabledData;
      }
      else
      {
        this.rbDataEnableEdit.Text = i_Tree_Eco_v6.Resources.Strings.EditingDisabled;
        this.rbDataEnableEdit.ToolTip = i_Tree_Eco_v6.Resources.Strings.ToolTipEditingDisabledData;
      }
      this.rbCheckBoxUID.Visible = y.RecordTreeUserId;
      this.rbDataShrubs.Visible = this.m_series.IsSample && y.RecordShrub;
      this.InitPlotOptions();
    }

    private void InitPlotOptions()
    {
      if (this.m_series == null)
        return;
      if (this.m_series.IsSample)
      {
        if (this.m_session != null && this.m_year != null)
        {
          TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
          this.m_taskManager.Add(Task.Factory.StartNew<bool>((Func<bool>) (() =>
          {
            using (ITransaction transaction = this.m_session.BeginTransaction())
            {
              try
              {
                int num = this.m_session.CreateCriteria<Eco.Domain.v6.Plot>().Add(Restrictions.Where<Eco.Domain.v6.Plot>((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Plot, bool>>) (p => p.Year == this.m_year))).SetProjection(Projections.RowCount()).UniqueResult<int>();
                transaction.Commit();
                return num > 0;
              }
              catch (HibernateException ex)
              {
                transaction.Rollback();
                throw;
              }
            }
          }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<bool>>) (t =>
          {
            if (this.IsDisposed || t.IsFaulted)
              return;
            this.rbDataShrubs.Enabled = t.Result;
            this.rbDataTrees.Enabled = t.Result;
            this.rbDataReferenceObjects.Enabled = t.Result && this.m_year.RecordReferenceObjects;
            this.rbDataGroundCovers.Enabled = t.Result && this.m_year.RecordGroundCover;
            this.rbDataLandUses.Enabled = t.Result && this.m_year.RecordLanduse;
          }), scheduler));
        }
        else
        {
          this.rbDataShrubs.Enabled = false;
          this.rbDataTrees.Enabled = false;
        }
      }
      else
        this.rbDataTrees.Enabled = true;
    }

    private void InitForecasts()
    {
      if (this.m_forecasts != null && this.m_forecasts.Count > 0)
      {
        foreach (Forecast forecast in (IEnumerable<Forecast>) this.m_forecasts)
        {
          RibbonItemCollection items = this.ConfigurationComboBox.Items;
          RibbonButton ribbonButton = new RibbonButton();
          ribbonButton.Text = forecast.Title;
          ribbonButton.Tag = (object) forecast.Guid;
          items.Add((RibbonItem) ribbonButton);
        }
      }
      else
      {
        Forecast forecast = this.addDefaultForecast();
        RibbonButton ribbonButton = new RibbonButton();
        ribbonButton.Text = forecast.Title;
        ribbonButton.Tag = (object) forecast.Guid;
        this.ConfigurationComboBox.Items.Add((RibbonItem) ribbonButton);
      }
      this.ConfigurationComboBox.SelectedIndex = 0;
      this.ConfigurationComboBox.Enabled = true;
    }

    private void InitForecastOptions()
    {
      Forecast forecast = this.m_forecast;
      this.rtForecast.Enabled = forecast != null && this._locationValid && this.m_year.RevProcessed > 0;
      bool flag = forecast != null && !forecast.Changed;
      this.ForecastEditDataButton.Enabled = flag;
      if (flag)
      {
        this.ForecastEditDataButton.Text = i_Tree_Eco_v6.Resources.Strings.EditingEnabled;
        this.ForecastEditDataButton.ToolTip = i_Tree_Eco_v6.Resources.Strings.ToolTipEditingEnabledForecast;
      }
      else
      {
        this.ForecastEditDataButton.Text = i_Tree_Eco_v6.Resources.Strings.EditingDisabled;
        this.ForecastEditDataButton.ToolTip = i_Tree_Eco_v6.Resources.Strings.ToolTipEditingDisabledForecast;
      }
      this.rgForecastReports.Enabled = flag;
      this.ForecastLockGroup.Enabled = flag;
      this.DefaultsButton.Enabled = flag && !this.isDefault(forecast);
    }

    private List<string> GetBreadcrumbPath(RibbonButton rb)
    {
      List<string> breadcrumbPath = new List<string>();
      breadcrumbPath.Add(rb.Text);
      object obj = rb.Parent;
      while (obj != null)
      {
        RibbonMenu ribbonMenu = obj as RibbonMenu;
        if ((RibbonItem) ribbonMenu != (RibbonItem) null)
        {
          for (int index = ribbonMenu.Items.IndexOf((RibbonItem) rb); index >= 0; --index)
          {
            RibbonLabel ribbonLabel = ribbonMenu.Items[index] as RibbonLabel;
            if ((RibbonItem) ribbonLabel != (RibbonItem) null)
            {
              breadcrumbPath.Add(ribbonLabel.Text);
              break;
            }
          }
          obj = ribbonMenu.Parent;
          breadcrumbPath.Add(ribbonMenu.Text);
        }
        RibbonGroup ribbonGroup = obj as RibbonGroup;
        if (ribbonGroup != (RibbonGroup) null)
        {
          if (!string.IsNullOrWhiteSpace(ribbonGroup.Text))
            breadcrumbPath.Add(ribbonGroup.Text);
          RibbonTab tab = ribbonGroup.Tab;
          breadcrumbPath.Add(tab.Text);
          obj = (object) null;
        }
        RibbonApplicationMenu ribbonApplicationMenu = obj as RibbonApplicationMenu;
        if ((RibbonItem) ribbonApplicationMenu != (RibbonItem) null)
        {
          if (!string.IsNullOrEmpty(ribbonApplicationMenu.Text))
            breadcrumbPath.Add(ribbonApplicationMenu.Text);
          obj = (object) null;
        }
        if (ribbonGroup == (RibbonGroup) null && (RibbonItem) ribbonMenu == (RibbonItem) null && (RibbonItem) ribbonApplicationMenu == (RibbonItem) null)
          obj = (object) null;
      }
      breadcrumbPath.Reverse();
      return breadcrumbPath;
    }

    private void ShowDocument(DockContent document, DockState state, object sender)
    {
      if (document is ProjectContentForm && document is ProjectOverviewForm)
        (document as ProjectOverviewForm).FormClosed += new FormClosedEventHandler(this.ProjectOverviewClosed);
      if (document is ContentForm)
      {
        ContentForm contentForm = document as ContentForm;
        contentForm.ShowHelp += new EventHandler<ShowHelpEventArgs>(this.ContentForm_ShowHelp);
        contentForm.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
        RibbonButton rb = sender as RibbonButton;
        if ((RibbonItem) rb != (RibbonItem) null)
        {
          List<string> breadcrumbPath = this.GetBreadcrumbPath(rb);
          if (breadcrumbPath.Count > 0)
          {
            contentForm.lblBreadcrumb.Visible = true;
            contentForm.Breadcrumbs = breadcrumbPath;
          }
          else
            contentForm.lblBreadcrumb.Visible = false;
        }
        else
          contentForm.lblBreadcrumb.Visible = false;
      }
      document.Show(this.dockPanel1, state);
    }

    private T ShowDocument<T>(object sender, params object[] args) where T : DockContent
    {
      DockContent document = this.dockPanel1.ActiveDocument as DockContent;
      Type type = typeof (T);
      if (document == null || document.GetType().FullName != type.FullName)
      {
        if (this.CloseAllDocuments())
        {
          document = Activator.CreateInstance(type, args) as DockContent;
          this.ShowDocument(document, DockState.Document, sender);
          if (!string.IsNullOrEmpty(document.Tag as string))
            this.ShowHelp(document.Tag as string);
          else
            this.ShowHelp(type.Name);
        }
      }
      else if (document is ContentForm)
      {
        ContentForm contentForm = document as ContentForm;
        RibbonButton rb = sender as RibbonButton;
        if ((RibbonItem) rb != (RibbonItem) null)
        {
          List<string> breadcrumbPath = this.GetBreadcrumbPath(rb);
          if (breadcrumbPath.Count > 0)
          {
            contentForm.lblBreadcrumb.Visible = true;
            contentForm.Breadcrumbs = breadcrumbPath;
          }
          else
            contentForm.lblBreadcrumb.Visible = false;
        }
        else
          contentForm.lblBreadcrumb.Visible = false;
      }
      return document as T;
    }

    private void ShowHelp(string topic)
    {
      if (this.m_ps == null)
        this.m_ps = ProgramSession.GetInstance();
      if (this.m_frmHelp == null)
      {
        this.m_frmHelp = new HelpForm();
        if (this.m_ps.MinimizeHelpPanel)
          this.m_frmHelp.Show(this.dockPanel1, DockState.DockLeftAutoHide);
        else
          this.m_frmHelp.Show(this.dockPanel1, DockState.DockLeft);
      }
      else if (this.m_ps.MinimizeHelpPanel)
        this.m_frmHelp.DockState = DockState.DockLeftAutoHide;
      else
        this.m_frmHelp.DockState = DockState.DockLeft;
      this.m_frmHelp.DisplayHelp(topic);
    }

    public void showTabSplash()
    {
      if (this.CloseAllDocuments())
      {
        ResourceManager resourceManager1 = TabPageSplashImageRes.ResourceManager;
        ResourceManager resourceManager2 = TabPageSplashTextRes.ResourceManager;
        if (this.m_showSplashTabSplash)
        {
          if (this.ShowDocument<TabSplash>((object) this, (object) (resourceManager1.GetObject("LaunchSplash") as Image), (object) "i-Tree Eco", (object) resourceManager2.GetString("LaunchSplash"), (object) true) != null)
            this.ShowHelp(this.m_curTab.Name);
          this.m_showSplashTabSplash = false;
        }
        else
        {
          string name = this.c1Ribbon1.SelectedTab.Name;
          string str = (string) null;
          if (this.m_series != null)
            str = !this.m_series.IsSample ? resourceManager2.GetString(name + "_full") : resourceManager2.GetString(name + "_plot");
          if (str == null)
            str = resourceManager2.GetString(name);
          if (this.ShowDocument<TabSplash>((object) this, (object) (resourceManager1.GetObject(this.c1Ribbon1.SelectedTab.Name) as Image), (object) this.c1Ribbon1.SelectedTab.Text, (object) str, (object) true) != null)
            this.m_curTab = this.c1Ribbon1.SelectedTab;
          this.ShowHelp(this.m_curTab.Name);
        }
      }
      else
      {
        this.m_tabChangeCanceled = true;
        this.c1Ribbon1.SelectedTab = this.m_curTab;
      }
    }

    private void UpdateProjectExports()
    {
      if (this.dockPanel1.ActiveDocument is IExportable activeDocument)
      {
        this.rgProjectExport.Enabled = true;
        this.rbProjectExportCSV.Enabled = activeDocument.CanExport(ExportFormat.CSV);
      }
      else
        this.rgProjectExport.Enabled = false;
    }

    private void UpdateDataExports()
    {
      if (this.dockPanel1.ActiveDocument is IExportable activeDocument)
      {
        this.rgDataExport.Enabled = true;
        this.rbDataExportCSV.Enabled = activeDocument.CanExport(ExportFormat.CSV);
        this.rbDataExportKML.Enabled = activeDocument.CanExport(ExportFormat.KML);
      }
      else
        this.rgDataExport.Enabled = false;
    }

    private void UpdateDataActions()
    {
      IActionable activeDocument = this.dockPanel1.ActiveDocument as IActionable;
      bool flag = this.dockPanel1.ActiveDocument is IImport;
      if (activeDocument != null)
      {
        this.rgDataActions.Visible = this.m_year != null && this.m_year.Changed;
        if (this.rgDataActions.Visible)
        {
          this.rgDataActions.Enabled = true;
          this.rbDataNew.Enabled = activeDocument.CanPerformAction(UserActions.New);
          this.rbDataCopy.Enabled = activeDocument.CanPerformAction(UserActions.Copy);
          this.rbDataUndo.Enabled = activeDocument.CanPerformAction(UserActions.Undo);
          this.rbDataRedo.Enabled = activeDocument.CanPerformAction(UserActions.Redo);
          this.rbDataDefaults.Enabled = activeDocument.CanPerformAction(UserActions.RestoreDefaults);
          this.rbDataDelete.Enabled = activeDocument.CanPerformAction(UserActions.Delete);
          this.rbImport.Enabled = flag && activeDocument.CanPerformAction(UserActions.ImportData);
        }
        else
          this.rbImport.Enabled = false;
      }
      else
      {
        this.rgDataActions.Visible = false;
        this.rbImport.Enabled = false;
      }
    }

    private void UpdateProjectActions()
    {
      if (this.dockPanel1.ActiveDocument is IActionable activeDocument)
      {
        this.rgProjectActions.Visible = this.m_year != null && this.m_year.ConfigEnabled;
        if (!this.rgProjectActions.Visible)
          return;
        this.rgProjectActions.Enabled = true;
        this.rbProjectNew.Enabled = activeDocument.CanPerformAction(UserActions.New);
        this.rbProjectCopy.Enabled = activeDocument.CanPerformAction(UserActions.Copy);
        this.rbProjectUndo.Enabled = activeDocument.CanPerformAction(UserActions.Undo);
        this.rbProjectRedo.Enabled = activeDocument.CanPerformAction(UserActions.Redo);
        this.rbProjectDefaults.Enabled = activeDocument.CanPerformAction(UserActions.RestoreDefaults);
        this.rbProjectDelete.Enabled = activeDocument.CanPerformAction(UserActions.Delete);
      }
      else
      {
        this.rgProjectActions.Visible = false;
        this.rgProjectActions.Enabled = false;
      }
    }

    private bool CloseAllDocuments()
    {
      IDockContent[] array = this.dockPanel1.DocumentsToArray();
      this.m_closingDocuments = true;
      foreach (Form form in array)
        form.Close();
      if (this.currMap != null)
      {
        this.currMap.Close();
        this.currMap = (Form) null;
      }
      this.rbMapData.Enabled = false;
      this.rbReportsExportKML.Enabled = false;
      this.rbReportsExportCSV.Enabled = false;
      this.m_closingDocuments = false;
      return this.dockPanel1.DocumentsCount == 0;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      if (MainRibbonForm.splashForm != null)
        MainRibbonForm.splashForm.Invoke((Delegate) (() => MainRibbonForm.splashForm.CloseSplashForm()));
      base.OnClosing(e);
    }

    private void rbtnOverview_Click(object sender, EventArgs e)
    {
      if (!this.m_ps.InputSession.YearKey.HasValue)
      {
        NewProjectType newProjectType = new NewProjectType();
        if (newProjectType.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        {
          this.ProjectOverviewClosed((object) this, new FormClosedEventArgs(CloseReason.None));
        }
        else
        {
          this.Text = string.Format("i-Tree Eco v{0}", (object) new Version(Application.ProductVersion).ToString(3));
          if (this.ShowDocument<ProjectOverviewForm>((object) this.rbtnOverview, (object) newProjectType.NewProjectSampleType) != null)
            this.ShowHelp("tpProjectInfo");
          this.c1Ribbon1.Enabled = false;
        }
      }
      else
      {
        if (this.ShowDocument<ProjectOverviewForm>(sender) == null)
          return;
        this.ShowHelp("tpProjectInfo");
      }
    }

    private void rbtnLandUses_Click(object sender, EventArgs e) => this.ShowDocument<LandUsesForm>(sender);

    private void rbtnGroundCovers_Click(object sender, EventArgs e) => this.ShowDocument<GroundCoversForm>(sender);

    private void rbDBHClasses_Click(object sender, EventArgs e) => this.ShowDocument<DBHRptClassesForm>(sender);

    private void rbCondition_Click(object sender, EventArgs e) => this.ShowDocument<ConditionsForm>(sender);

    private void rbDieback_Click(object sender, EventArgs e) => this.ShowDocument<DiebacksForm>(sender);

    private void rbtnStreets_Click(object sender, EventArgs e) => this.ShowDocument<StreetsForm>(sender);

    private void rbtnLocSites_Click(object sender, EventArgs e) => this.ShowDocument<LookupForm<LocSite>>(sender, (object) v6Strings.LocSite_SingularName, (object) v6Strings.LocSite_PluralName, (object) "LocationSitesForm");

    private void rbPlantingSiteTypes_Click(object sender, EventArgs e) => this.ShowDocument<PlantingSiteTypesForm>(sender);

    private void rbtnMaintRec_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<LookupForm<MaintRec>>(sender, (object) v6Strings.MaintRec_SingularName, (object) v6Strings.MaintRec_PluralName, (object) "MaintenanceForm") == null)
        return;
      this.ShowHelp("MaintenanceRecommendedForm");
    }

    private void rbtnMaintTasks_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<LookupForm<Eco.Domain.v6.MaintTask>>(sender, (object) v6Strings.MaintTask_SingularName, (object) v6Strings.MaintTask_PluralName, (object) "MaintenanceForm") == null)
        return;
      this.ShowHelp("MaintenanceTaskForm");
    }

    private void rbSidewalkConflict_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<LookupForm<Sidewalk>>(sender, (object) v6Strings.Sidewalk_SingularName, (object) v6Strings.Sidewalk_PluralName, (object) "SidewalkConflictForm") == null)
        return;
      this.ShowHelp("SidewalkConflictForm");
    }

    private void rbUtilityConflicts_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<LookupForm<Eco.Domain.v6.WireConflict>>(sender, (object) v6Strings.WireConflict_SingularName, (object) v6Strings.WireConflict_PluralName, (object) "UtilityConflictForm") == null)
        return;
      this.ShowHelp("UtilityConflictForm");
    }

    private void rbtnOtherOne_Click(object sender, EventArgs e)
    {
      Year year = this.m_year;
      if (year == null)
        return;
      string str = year.OtherOne;
      if (string.IsNullOrEmpty(str))
        str = v6Strings.FieldOne_SingularName;
      if (this.ShowDocument<LookupForm<OtherOne>>(sender, (object) str, null, (object) "CustomFieldsForm") == null)
        return;
      this.ShowHelp("CustomFieldsForm");
    }

    private void rbtnOtherTwo_Click(object sender, EventArgs e)
    {
      Year year = this.m_year;
      if (year == null)
        return;
      string str = year.OtherTwo;
      if (string.IsNullOrEmpty(str))
        str = v6Strings.FieldTwo_SingularName;
      if (this.ShowDocument<LookupForm<OtherTwo>>(sender, (object) str, null, (object) "CustomFieldsForm") == null)
        return;
      this.ShowHelp("CustomFieldsForm");
    }

    private void rbtnOtherThree_Click(object sender, EventArgs e)
    {
      Year year = this.m_year;
      if (year == null)
        return;
      string str = year.OtherThree;
      if (string.IsNullOrEmpty(str))
        str = v6Strings.FieldThree_SingularName;
      if (this.ShowDocument<LookupForm<OtherThree>>(sender, (object) str, null, (object) "CustomFieldsForm") == null)
        return;
      this.ShowHelp("CustomFieldsForm");
    }

    private void rbtnBenefitPrices_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<BenefitPricesForm>(sender) == null)
        return;
      this.ShowHelp("BenefitPricesForm");
    }

    private void loadPlotsFromFileButton_Click(object sender, EventArgs e) => this.ShowDocument<DefinePlotsFileForm>(sender);

    private void definePlotsViaGoogleMaps_Click(object sender, EventArgs e) => this.ShowDocument<DefinePlotsViaGoogleMapsForm>(sender);

    private void definePlotsManuallyButton_Click(object sender, EventArgs e) => this.ShowDocument<DefinePlotsManuallyForm>(sender);

    private void rbStrata_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<StrataForm>(sender) == null)
        return;
      if (this.m_series == null || this.m_series.IsSample)
        this.ShowHelp("StrataForm_plot");
      else
        this.ShowHelp("StrataForm_full");
    }

    private void rbtnPaperForm1_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<PaperForm>(sender) == null)
        return;
      this.ShowHelp("PaperForms");
    }

    private void rbtnPrint1_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<PaperForm>(sender) == null)
        return;
      this.ShowHelp("PrintPaperForms");
    }

    private void rbProjectEnableEdit_Click(object sender, EventArgs e)
    {
      this.ShowHelp("EnableEditing_ProjectConfig");
      this.EnableEditing(true);
    }

    private void rbDataEnableEdit_Click(object sender, EventArgs e)
    {
      this.ShowHelp("EnableEditing_Data");
      this.EnableEditing(false);
    }

    private void EnableEditing(bool editConfig)
    {
      if (!this.m_year.Changed && RTFMessageBox.Show(ApplicationHelp.EnableEditing, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;
      if (editConfig && (!string.IsNullOrEmpty(this.m_year.MobileKey) || this.m_year.RevProcessed > 0))
      {
        if (RTFMessageBox.Show(ApplicationHelp.EnableConfig, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Hand, ApplicationHelp.AcceptEnableEditing) == DialogResult.No)
          return;
        this.m_year.MobileKey = (string) null;
        this.m_year.RevProcessed = 0;
        this.m_forecast.Changed = true;
      }
      this.m_year.Changed = true;
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        try
        {
          this.m_session.SaveOrUpdate((object) this.m_year);
          IList<Forecast> forecastList = this.m_session.CreateCriteria<Forecast>().Add((ICriterion) Restrictions.Eq("Year", (object) this.m_year)).List<Forecast>();
          foreach (Forecast forecast in (IEnumerable<Forecast>) forecastList)
          {
            forecast.Changed = true;
            this.m_session.SaveOrUpdate((object) forecast);
          }
          transaction.Commit();
          EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.m_year), (Control) this);
          foreach (Forecast entity in (IEnumerable<Forecast>) forecastList)
            EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(entity), (Control) this);
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      this.InitYearOptions();
      this.InitForecastOptions();
    }

    private void rbSubmitMobileData_Click(object sender, EventArgs e)
    {
      if (this.m_series.IsSample)
        this.ShowDocument<MobileSubmitSampleForm>(sender);
      else
        this.ShowDocument<MobileSubmitInventoryForm>(sender);
    }

    private void rbRetrieveMobileData_Click(object sender, EventArgs e) => this.ShowDocument<MobileRetrieveDataForm>(sender);

    private void rbtnDataPlots_Click(object sender, EventArgs e) => this.ShowDocument<PlotsForm>(sender);

    private void rbDataTrees_Click(object sender, EventArgs e)
    {
      if (this.m_series.IsSample)
        this.ShowDocument<SampleTreesForm>(sender);
      else
        this.ShowDocument<InventoryTreesForm>(sender);
    }

    private void rbDataShrubs_Click(object sender, EventArgs e) => this.ShowDocument<ShrubsForm>(sender);

    private void rbDataPlantingSites_Click(object sender, EventArgs e) => this.ShowDocument<PlantingSitesForm>(sender);

    private void rgActions_DialogLauncherClick(object sender, EventArgs e) => this.ShowHelp("UserActions");

    private void rgActions1_DialogLauncherClick(object sender, EventArgs e) => this.ShowHelp("UserActions");

    private void rbDataNew_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      ((ContainerControl) this.dockPanel1.ActiveDocument).Validate();
      activeDocument.PerformAction(UserActions.New);
      this.UpdateDataActions();
    }

    private void rbDataCopy_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      ((ContainerControl) this.dockPanel1.ActiveDocument).Validate();
      activeDocument.PerformAction(UserActions.Copy);
      this.UpdateDataActions();
    }

    private void rbDataUndo_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.Undo);
      this.UpdateDataActions();
    }

    private void rbDataRedo_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.Redo);
      this.UpdateDataActions();
    }

    private void rbDataDefaults_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.RestoreDefaults);
      this.UpdateProjectActions();
    }

    private void rbDataDelete_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.Delete);
      this.UpdateDataActions();
    }

    private void rbProjectNew_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.New);
      this.UpdateProjectActions();
    }

    private void rbProjectCopy_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.Copy);
      this.UpdateProjectActions();
    }

    private void rbProjectUndo_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.Undo);
      this.UpdateProjectActions();
    }

    private void rbProjectRedo_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.Redo);
      this.UpdateProjectActions();
    }

    private void rbProjectDefaults_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.RestoreDefaults);
      this.UpdateProjectActions();
    }

    private void rbProjectDelete_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IActionable activeDocument))
        return;
      activeDocument.PerformAction(UserActions.Delete);
      this.UpdateProjectActions();
    }

    private void rbtnPrint_Click(object sender, EventArgs e) => this.ShowHelp("Print");

    private void rcboProject_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.m_selectingProject)
        return;
      this.rcboSeries.Items.Clear();
      this.rcboSeries.Text = string.Empty;
      this.rcboYear.Items.Clear();
      this.rcboYear.Text = string.Empty;
      this.ConfigurationComboBox.Items.Clear();
      this.ConfigurationComboBox.Text = string.Empty;
      this.rcboSeries.Enabled = false;
      this.rcboYear.Enabled = false;
      this.ConfigurationComboBox.Enabled = false;
      this.m_ps.InputSession.YearKey = new Guid?();
      this.m_ps.InputSession.ForecastKey = new Guid?();
      Eco.Domain.v6.Project tag = (Eco.Domain.v6.Project) this.rcboProject.SelectedItem.Tag;
      LocationSpecies.Domain.Location location = this._locationService.GetLocation(tag.LocationId);
      lock (this.m_syncObj)
      {
        this.m_project = tag;
        this._locationValid = location != null;
      }
      if (!this._locationValid)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrLocationUnavailable, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.LoadFrostFreeDays(tag.LocationId);
      this.m_taskManager.Add(this.InitSeries(tag));
    }

    private void rcboSeries_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.rcboSeries.SelectedIndex == -1 || this.m_selectingProject)
        return;
      this.rcboYear.Items.Clear();
      this.rcboYear.Text = string.Empty;
      this.ConfigurationComboBox.Items.Clear();
      this.ConfigurationComboBox.Text = string.Empty;
      this.rcboYear.Enabled = false;
      this.ConfigurationComboBox.Enabled = false;
      this.m_ps.InputSession.YearKey = new Guid?();
      this.m_ps.InputSession.ForecastKey = new Guid?();
      Eco.Domain.v6.Series tag = (Eco.Domain.v6.Series) this.rcboSeries.SelectedItem.Tag;
      lock (this.m_syncObj)
        this.m_series = tag;
      this.InitSeriesOptions();
      this.m_taskManager.Add(this.InitYears(tag));
    }

    private Task InitYears(Eco.Domain.v6.Series series)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew<IList<Year>>((Func<IList<Year>>) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          try
          {
            IList<Year> yearList = this.m_session.CreateCriteria<Year>().Add((ICriterion) Restrictions.Eq("Series", (object) series)).List<Year>();
            transaction.Commit();
            return yearList;
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<IList<Year>>>) (t =>
      {
        if (this.IsDisposed || t.IsFaulted)
          return;
        this.rcboYear.Items.Clear();
        foreach (Year year in (IEnumerable<Year>) t.Result)
        {
          RibbonItemCollection items = this.rcboYear.Items;
          items.Add((RibbonItem) new RibbonButton()
          {
            Text = year.Id.ToString(),
            Tag = (object) year.Guid
          });
        }
        this.rcboYear.SelectedIndex = 0;
        this.rcboYear.Enabled = true;
        this.Text = string.Format("[{0}: {1}] [{2}: {3}] [{4}: {5}] - i-Tree Eco v{6}", (object) v6Strings.Project_SingularName, (object) this.rcboProject.Text, (object) v6Strings.Series_SingularName, (object) this.rcboSeries.Text, (object) v6Strings.Year_SingularName, (object) this.rcboYear.Text, (object) new Version(Application.ProductVersion).ToString(3));
      }), scheduler);
    }

    private void rcboYear_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.rcboYear.SelectedIndex == -1 || this.m_selectingProject)
        return;
      this.ConfigurationComboBox.Items.Clear();
      this.ConfigurationComboBox.Enabled = false;
      this.m_ps.InputSession.ForecastKey = new Guid?();
      this.Text = string.Format("[{0}: {1}] [{2}: {3}] [{4}: {5}] - i-Tree Eco v{6}", (object) v6Strings.Project_SingularName, (object) this.rcboProject.Text, (object) v6Strings.Series_SingularName, (object) this.rcboSeries.Text, (object) v6Strings.Year_SingularName, (object) this.rcboYear.Text, (object) new Version(Application.ProductVersion).ToString(3));
      this.m_ps.InputSession.YearKey = new Guid?((Guid) this.rcboYear.SelectedItem.Tag);
    }

    private void rbtnPlotRefObjects_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is PlotsForm activeDocument))
        return;
      activeDocument.ShowReferenceObjects();
    }

    private void rbtnPlotLandUses_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is PlotsForm activeDocument))
        return;
      activeDocument.ShowLandUses();
    }

    private void rbtnPlotGroundCover_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is PlotsForm activeDocument))
        return;
      activeDocument.ShowGroundCovers();
    }

    private void rbtnPlotTrees_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is PlotsForm activeDocument))
        return;
      activeDocument.ShowTrees();
    }

    private void rbtnPlotShrubs_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is PlotsForm activeDocument))
        return;
      activeDocument.ShowShrubs();
    }

    private void rbtnPlotPlantingSites_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is PlotsForm activeDocument))
        return;
      activeDocument.ShowPlantingSites();
    }

    private void RibbonGroup_MeasureItem(RibbonGroup rg, C1.Win.C1Ribbon.MeasureItemEventArgs e)
    {
      RibbonStyle ribbonStyle = e.Item.Ribbon.RibbonStyle;
      int num = 0;
      foreach (RibbonItem ribbonItem in (CollectionBase) rg.Items)
      {
        RibbonButton ribbonButton = ribbonItem as RibbonButton;
        if ((RibbonItem) ribbonButton != (RibbonItem) null)
        {
          Size size = TextRenderer.MeasureText(ribbonButton.Text, ribbonButton.Ribbon.Font);
          if (size.Width > num)
            num = size.Width;
        }
      }
      e.ItemHeight = ribbonStyle.ConstSet[StyleConst.FormButtonHeight];
      e.ItemWidth = num + 2 * ribbonStyle.ConstSet[StyleConst.FormTextOffsetX];
    }

    private void RibbonGroup_DrawItem(C1.Win.C1Ribbon.DrawItemEventArgs e)
    {
      RibbonToggleButton ribbonToggleButton = e.Item as RibbonToggleButton;
      Rectangle bounds = e.Bounds;
      float left = (float) bounds.Left;
      float top = (float) bounds.Top;
      float num1 = (float) (bounds.Width - 1);
      float num2 = (float) (bounds.Height - 1);
      if ((e.State & DrawItemState.Selected) != DrawItemState.None)
      {
        GraphicsPath path = new GraphicsPath();
        float num3 = 3f;
        float num4 = left + num1;
        float num5 = top + num2;
        float num6 = num4 - num3;
        float num7 = num5 - num3;
        float num8 = left + num3;
        float num9 = top + num3;
        float num10 = num3 * 2f;
        float x = num4 - num10;
        float y = num5 - num10;
        path.AddArc(left, top, num10, num10, 180f, 90f);
        path.AddLine(num8, top, num6, top);
        path.AddArc(x, top, num10, num10, 270f, 90f);
        path.AddLine(num4, num9, num4, num7);
        path.AddArc(x, y, num10, num10, 0.0f, 90f);
        path.AddLine(num6, num5, num8, num5);
        path.AddArc(left, y, num10, num10, 90f, 90f);
        path.AddLine(left, num7, left, num9);
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.FillPath((Brush) new SolidBrush(System.Drawing.Color.FromArgb(128, 156, 207, 115)), path);
        e.Graphics.DrawPath(new Pen(System.Drawing.Color.DarkBlue), path);
      }
      SizeF sizeF = e.Graphics.MeasureString(ribbonToggleButton.Text, ribbonToggleButton.Ribbon.Font);
      float x1 = left + (float) (((double) num1 - (double) sizeF.Width) / 2.0);
      float y1 = top + (float) (((double) num2 - (double) sizeF.Height) / 2.0);
      e.Graphics.DrawString(ribbonToggleButton.Text, ribbonToggleButton.Ribbon.Font, (Brush) new SolidBrush(e.ForeColor), x1, y1);
    }

    private void rgSpecies_MeasureItem(object sender, C1.Win.C1Ribbon.MeasureItemEventArgs e) => this.RibbonGroup_MeasureItem(this.rgSpeciesName, e);

    private void rgSpecies_DrawItem(object sender, C1.Win.C1Ribbon.DrawItemEventArgs e) => this.RibbonGroup_DrawItem(e);

    private void rgSpecies_PressedChanged(object sender, EventArgs e)
    {
      if (this.rbSpeciesSN.Pressed && !this.rbReportsSpeciesSN.Pressed)
        this.rbReportsSpeciesSN.Pressed = true;
      if (this.rbSpeciesCN.Pressed && !this.rbReportsSpeciesCN.Pressed)
        this.rbReportsSpeciesCN.Pressed = true;
      this.m_ps.SpeciesDisplayName = !this.rbSpeciesCN.Pressed ? SpeciesDisplayEnum.ScientificName : SpeciesDisplayEnum.CommonName;
      if (this.dockPanel1.ActiveDocument is ReportViewerForm activeDocument1)
        activeDocument1.RefreshReport();
      if (!(this.dockPanel1.ActiveDocument is IPEDReportForm activeDocument2))
        return;
      activeDocument2.RunReport();
    }

    private void rgUnits_MeasureItem(object sender, C1.Win.C1Ribbon.MeasureItemEventArgs e) => this.RibbonGroup_MeasureItem(this.rgUnits, e);

    private void rgUnits_DrawItem(object sender, C1.Win.C1Ribbon.DrawItemEventArgs e) => this.RibbonGroup_DrawItem(e);

    private void rgUnits_PressedChanged(object sender, EventArgs e)
    {
      this.m_ps.UseEnglishUnits = this.rbEnglish.Pressed;
      if (this.rbEnglish.Pressed && !this.rtbForecastEnglish.Pressed)
        this.rtbForecastEnglish.Pressed = true;
      if (this.rbMetric.Pressed && !this.rtbForecastMetric.Pressed)
        this.rtbForecastMetric.Pressed = true;
      if (this.dockPanel1.ActiveDocument is ReportViewerForm activeDocument1)
        activeDocument1.RefreshReport();
      if (this.dockPanel1.ActiveDocument is WeatherChart activeDocument2)
        activeDocument2.RefreshChart();
      if (this.dockPanel1.ActiveDocument is ChartViewerForm activeDocument3)
        activeDocument3.MakeChart();
      if (!(this.dockPanel1.ActiveDocument is ChartViewerFormForVOC activeDocument4))
        return;
      activeDocument4.MakeChart();
    }

    private void rbtnSpeciesCode_Click(object sender, EventArgs e) => this.ShowDocument<SpeciesCodesForm>(sender);

    private void rcbAutoCheckUpdates_CheckedChanged(object sender, EventArgs e) => this.m_ps.AutoCheckUpdates = this.rcbAutoCheckUpdates.Checked;

    private async void runModelsButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("RunModels");
      Eco.Domain.v6.Project project = this.m_ps.InputSession.CreateSession().Get<Year>((object) this.m_ps.InputSession.YearKey).Series.Project;
      int theLocationID = project.LocationId;
      ProjectLocation projectLocation = project.Locations.Where<ProjectLocation>((Func<ProjectLocation, bool>) (p => p.LocationId == theLocationID)).First<ProjectLocation>();
      if (projectLocation.NationCode != "001" && projectLocation.NationCode != "230" && projectLocation.NationCode != "002" && projectLocation.NationCode != "021")
      {
        int num1 = (int) MessageBox.Show(EngineModelRes.NonAppropriateInternationalCountryWarningMessage, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
      }
      else if (projectLocation.PrimaryPartitionCode == "00" || projectLocation.SecondaryPartitionCode == "000" || projectLocation.TertiaryPartitionCode == "00000")
      {
        int num2 = (int) MessageBox.Show(EngineModelRes.LocationDetailsNotSet, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
      }
      else if (this.runModelsButton.Tag != null && this.runModelsButton.Tag.Equals((object) "running"))
        ;
      else
      {
        this.runModelsButton.Tag = (object) "running";
        if (!this.m_year.Changed && MessageBox.Show(i_Tree_Eco_v6.Resources.Strings.MsgConfirmRunModels, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          this.runModelsButton.Tag = (object) "";
        else if (MessageBox.Show(EcoCacheRes.CacheEula, EcoCacheRes.CacheTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
        {
          this.runModelsButton.Tag = (object) "";
        }
        else
        {
          frmEngineProgress engineProgressForm = new frmEngineProgress();
          CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
          engineProgressForm.engineCancelTokenSource = cancellationTokenSource;
          CancellationToken token = cancellationTokenSource.Token;
          Progress<EngineProgressArg> progress = new Progress<EngineProgressArg>();
          progress.ProgressChanged += (EventHandler<EngineProgressArg>) ((o, myArg) => engineProgressForm.DisplayProgressWithAnnimation(myArg));
          bool modelRunResult = false;
          EngineModel engineObj = new EngineModel();
          EngineProgressArg engineProgressArg = new EngineProgressArg();
          engineObj.engineProgressForm = engineProgressForm;
          engineObj.engineCancellationToken = token;
          engineObj.engineReportArg = engineProgressArg;
          engineObj.engineProgress = (IProgress<EngineProgressArg>) progress;
          engineProgressArg.TotalSteps = 12;
          try
          {
            modelRunResult = await engineObj.run_NonBackgroundPart(this.m_ps);
            if (!modelRunResult)
            {
              engineObj.deleteTempFiles();
              engineProgressForm.Close();
              this.runModelsButton.Tag = (object) "";
              return;
            }
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show(ex.Message);
            engineObj.deleteTempFiles();
            engineProgressForm.Close();
            this.runModelsButton.Tag = (object) "";
            return;
          }
          engineProgressForm.ShowCancelButton();
          await Task.Factory.StartNew((System.Action) (() =>
          {
            try
            {
              modelRunResult = engineObj.run_BackgroundPart();
              if (modelRunResult)
                return;
              engineObj.deleteTempFiles();
              this.runModelsButton.Tag = (object) "";
            }
            catch (Exception ex)
            {
              int num4 = (int) MessageBox.Show(ex.Message);
              engineObj.deleteTempFiles();
              modelRunResult = false;
              this.runModelsButton.Tag = (object) "";
            }
          }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);
          this.runModelsButton.Tag = (object) "";
          if (modelRunResult)
          {
            engineProgressForm.SetFinished();
            await Task.Factory.StartNew((System.Action) (() =>
            {
              this.m_year.Changed = false;
              this.m_forecast.Changed = true;
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                try
                {
                  this.m_session.SaveOrUpdate((object) this.m_year);
                  IList<Forecast> forecastList = this.m_session.CreateCriteria<Forecast>().Add((ICriterion) Restrictions.Eq("Year", (object) this.m_year)).List<Forecast>();
                  foreach (Forecast forecast in (IEnumerable<Forecast>) forecastList)
                  {
                    forecast.Changed = true;
                    this.m_session.SaveOrUpdate((object) forecast);
                  }
                  transaction.Commit();
                  EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.m_year), (Control) this);
                  foreach (Forecast entity in (IEnumerable<Forecast>) forecastList)
                    EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(entity), (Control) this);
                }
                catch (HibernateException ex)
                {
                  transaction.Rollback();
                  throw;
                }
              }
            }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);
            this.InitYearOptions();
            this.InitForecastOptions();
          }
          else
            engineProgressForm.Close();
        }
      }
    }

    private void projectMetadataButton_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<ProjectMetadata>)
        return;
      Report report = (Report) new Report<ProjectMetadata>();
      reportViewerForm.Report = report;
    }

    private void writtenReportButton_Click(object sender, EventArgs e)
    {
      if (this.m_year.YearLocationData.Count == 0 || this.m_year.YearLocationData.First<YearLocationData>().Population == 0)
      {
        int num = (int) MessageBox.Show("Please set location's population on Project Overview Form before running Written Report.");
      }
      else
      {
        ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
        if (reportViewerForm == null)
          return;
        if (!(reportViewerForm.Report is Report<WrittenReport>))
        {
          Report report = (Report) new Report<WrittenReport>();
          reportViewerForm.Report = report;
        }
        this.ShowHelp("WrittenReport");
      }
    }

    private void airQualityHealthImpactsandValuesbyTrees_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<AirQualityHealthImpactsAndValuesbyTrees>)
        return;
      Report report = (Report) new Report<AirQualityHealthImpactsAndValuesbyTrees>();
      reportViewerForm.Report = report;
    }

    private void airQualityHealthImpactsandValuesbyTreesandShrubs_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<AirQualityHealthImpactsAndValuesbyTreesAndShrubs>)
        return;
      Report report = (Report) new Report<AirQualityHealthImpactsAndValuesbyTreesAndShrubs>();
      reportViewerForm.Report = report;
    }

    private void carbonStorageOfTreesByStrataChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<CarbonStorageOfTreesByStrata>))
      {
        Report report = (Report) new Report<CarbonStorageOfTreesByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("CarbonStorageStrataReport");
    }

    private void carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<CarbonStorageOfTreesByStrataPerUnitArea>))
      {
        Report report = (Report) new Report<CarbonStorageOfTreesByStrataPerUnitArea>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("CarbonStorageAreaReport");
    }

    private void annualCarbonSequestrationOfTreesByStrataChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<CarbonSequestrationByStrata>))
      {
        Report report = (Report) new Report<CarbonSequestrationByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("CarbonSequestrationStrataReport");
    }

    private void annualCarbonSequestrationOfTreesBySpeciesChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<CarbonSequestrationBySpecies>))
      {
        Report report = (Report) new Report<CarbonSequestrationBySpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("CarbonSequestrationSpeciesReport");
    }

    private void annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<CarbonSequestrationByStrataPerUnitArea>))
      {
        Report report = (Report) new Report<CarbonSequestrationByStrataPerUnitArea>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("CarbonSequestrationAreaReport");
    }

    private void energyEffectsOfTreesMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<EnergyEffectsOfTrees>))
      {
        Report report = (Report) new Report<EnergyEffectsOfTrees>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("EnergyEffectsReport");
    }

    private void HydrologyEffectsOfTreesBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<i_Tree_Eco_v6.Reports.HydrologyEffectsOfTreesBySpecies>)
        return;
      Report report = (Report) new Report<i_Tree_Eco_v6.Reports.HydrologyEffectsOfTreesBySpecies>();
      reportViewerForm.Report = report;
    }

    private void HydrologyEffectsOfTreesByStratum_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<i_Tree_Eco_v6.Reports.HydrologyEffectsOfTreesByStratum>)
        return;
      Report report = (Report) new Report<i_Tree_Eco_v6.Reports.HydrologyEffectsOfTreesByStratum>();
      reportViewerForm.Report = report;
    }

    private void oxygenProductionOfTreesByStrataChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<OxygenProductionOfTreesByStrata>))
      {
        Report report = (Report) new Report<OxygenProductionOfTreesByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("OxygenProductionStrataReport");
    }

    private void oxygenProductionPerUnitAreaByStrataChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<OxygenProductionOfTreesByStrataPerUnitArea>))
      {
        Report report = (Report) new Report<OxygenProductionOfTreesByStrataPerUnitArea>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("OxygenProductionStrataAreaReport");
    }

    private void monthlyPollutantRemovalByTreesAndShrubsMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PollutantRemovalMonthlybyTreesAndShrubs>))
      {
        Report report = (Report) new Report<PollutantRemovalMonthlybyTreesAndShrubs>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("PollutionRemovalMonthlyReport");
    }

    private void monthlyPollutantRemovalByTreesAndShrubsChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PollutantRemovalMonthlybyTreesAndShrubsChart>))
      {
        Report report = (Report) new Report<PollutantRemovalMonthlybyTreesAndShrubsChart>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("PollutionRemovalMonthlyChart");
    }

    private void monthlyPollutantRemovalByGrassMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PollutantRemovalMonthlybyGrass>))
      {
        Report report = (Report) new Report<PollutantRemovalMonthlybyGrass>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("PollutantRemovalMonthlybyGrass");
    }

    private void monthlyPollutantRemovalByGrassChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PollutantRemovalMonthlybyGrassChart>))
      {
        Report report = (Report) new Report<PollutantRemovalMonthlybyGrassChart>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("PollutantRemovalMonthlybyGrassChart");
    }

    private void hourlyPollutantRemovalByTreesAndShrubsChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      this.ShowHelp("PollutionRemovalHourlyChart");
    }

    private void bioemissionsOfTreesBySpeciesMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<BioemissionsOfTreesBySpecies>))
      {
        Report report = (Report) new Report<BioemissionsOfTreesBySpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("BioemissionsSpeciesReport");
    }

    private void bioemissionsOfTreesByStrataMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<BioemissionsOfTreesByStrata>))
      {
        Report report = (Report) new Report<BioemissionsOfTreesByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("BioemissionsStrataReport");
    }

    private void numberOfTreesByStrataChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PopulationSummaryByStrata>))
      {
        Report report = (Report) new Report<PopulationSummaryByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("NumberOfTreesStrataReport");
    }

    private void numberOfTreesBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PopulationSummaryBySpecies>))
      {
        Report report = (Report) new Report<PopulationSummaryBySpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("PopulationSummarySpeciesReport");
    }

    private void numberOfTreesPerUnitAreaByStrataChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PopulationSummaryByStrataPerUnitArea>))
      {
        Report report = (Report) new Report<PopulationSummaryByStrataPerUnitArea>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("NumberOfTreesStrataAreaReport");
    }

    private void speciesDistributionByDBHClassChart_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<SpeciesDistributionByDBHClassChart>))
      {
        Report report = (Report) new Report<SpeciesDistributionByDBHClassChart>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("SpeciesCompositionDBHReport_chart");
    }

    private void speciesCompositionByDBHClassAndStrataMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<SpeciesCompositionByDBHByStrata>))
      {
        Report report = (Report) new Report<SpeciesCompositionByDBHByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("SpeciesCompositionDBHStrataReport_vert");
    }

    private void speciesCompositionByDBHClassAndStrataHorizontalMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<SpeciesCompositionByDBHByStrataHorizontal>))
      {
        Report report = (Report) new Report<SpeciesCompositionByDBHByStrataHorizontal>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("SpeciesCompositionDBHStrataReport_hor");
    }

    private void speciesCompositionByDBHClassMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<SpeciesCompositionByDBH>))
      {
        Report report = (Report) new Report<SpeciesCompositionByDBH>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("SpeciesCompositionDBHReport_vert");
    }

    private void speciesCompositionByDBHClassHorizontalMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<SpeciesCompositionByDBHHorizontal>))
      {
        Report report = (Report) new Report<SpeciesCompositionByDBHHorizontal>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("SpeciesCompositionDBHReport_hor");
    }

    private void mostImportantTreeSpeciesMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<MostImportantTreeSpecies>))
      {
        Report report = (Report) new Report<MostImportantTreeSpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("MostImportantTreeSpeciesReport");
    }

    private void speciesRichnessShannonWienerDiversityIndexMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<SpeciesRichnessShannonWienerDiversityIndexByStrata>))
      {
        Report report = (Report) new Report<SpeciesRichnessShannonWienerDiversityIndexByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("DiversityIndexReport");
    }

    private void originOfTreesByStrataMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<NativeStatusByStrata>))
      {
        Report report = (Report) new Report<NativeStatusByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("OriginOfTreesStrataReport");
    }

    private void conditionOfTreesBySpeciesMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<TreeConditionBySpecies>))
      {
        Report report = (Report) new Report<TreeConditionBySpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("TreeConditionSpeciesReport");
    }

    private void conditionOfTreesBySpeciesCustomMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<TreeConditionBySpeciesCustom>)
        return;
      Report report = (Report) new Report<TreeConditionBySpeciesCustom>();
      reportViewerForm.Report = report;
    }

    private void conditionOfTreesByStrataMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<TreeConditionByStrataSpecies>))
      {
        Report report = (Report) new Report<TreeConditionByStrataSpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("TreeConditionStrataReport");
    }

    private void conditionOfTreesByStratumSpeciesCustomMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<TreeConditionByStratumAndSpeciesCustom>)
        return;
      Report report = (Report) new Report<TreeConditionByStratumAndSpeciesCustom>();
      reportViewerForm.Report = report;
    }

    private void leafAreaAndBiomassOfShrubsByStrataMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<LeafAreaAndBiomassForShrubsByStrata>))
      {
        Report report = (Report) new Report<LeafAreaAndBiomassForShrubsByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("LeafAreaBiomassShrubsStrataReport");
    }

    private void leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<LeafAreaAndBiomassForTreesAndShrubsByStrata>))
      {
        Report report = (Report) new Report<LeafAreaAndBiomassForTreesAndShrubsByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("LeafAreaBiomassTreeShrubStrataReport");
    }

    private void leafAreaOfTreesByStrataChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<i_Tree_Eco_v6.Reports.LeafAreaByStrata>))
      {
        Report report = (Report) new Report<i_Tree_Eco_v6.Reports.LeafAreaByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("LeafAreaTreesStrataChart");
    }

    private void leafAreaOfTreesPerUnitAreaByStrataChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<LeafAreaByStrataPerUnitArea>))
      {
        Report report = (Report) new Report<LeafAreaByStrataPerUnitArea>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("LeafAreaStrataAreaTreesChart");
    }

    private void groundCoverCompositionByStrataMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PercentGroundCoverByStrata>))
      {
        Report report = (Report) new Report<PercentGroundCoverByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("GroundCoverCompositionStrataReport");
    }

    private void landUseCompositionByStrataMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<LandUseCompositionByStrata>))
      {
        Report report = (Report) new Report<LandUseCompositionByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("AccuracyOfStrataPreditionsReport");
    }

    private void rbMDDCompositionOfTrees_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeCharacteristics>))
      {
        Report report = (Report) new Report<IndividualTreeCharacteristics>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("InventorySummaryIndReport");
    }

    private void rbMDDTreeBenefitsSummary_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeBenefitsSummary>))
      {
        Report report = (Report) new Report<IndividualTreeBenefitsSummary>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("BenefitsSummaryIndReport");
    }

    private void rbMDDCompositionOfTreeBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<MeasuredTreeDetailsBySpecies>))
      {
        Report report = (Report) new Report<MeasuredTreeDetailsBySpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("InventorySummarySpeciesReport");
    }

    private void rbMDDTreeBenefitsPollutionRemoval_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreePollutionEffects>))
      {
        Report report = (Report) new Report<IndividualTreePollutionEffects>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("PollutionEffectsIndReport");
    }

    private void rbMDDTreeBenefitsEnergyEffects_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeEnergyEffects>))
      {
        Report report = (Report) new Report<IndividualTreeEnergyEffects>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("EnergyEffectsIndReport");
    }

    private void rbMDDTreeEnergyAvoidedPollutants_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<IndividualTreeEnergyAvoidedPollutants>)
        return;
      Report report = (Report) new Report<IndividualTreeEnergyAvoidedPollutants>();
      reportViewerForm.Report = report;
    }

    private void rbMDDTreeBenefitsHydrologyEffects_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeAvoidedRunoff>))
      {
        Report report = (Report) new Report<IndividualTreeAvoidedRunoff>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("AvoidedRunoffIndReport");
    }

    private void rbMDDTreeBenefitsOxygenProduction_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeOxygenProduction>))
      {
        Report report = (Report) new Report<IndividualTreeOxygenProduction>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("OxygenProductionIndReport");
    }

    private void rbMDDTreeBenefitsVOCEmissions_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeBioemissions>))
      {
        Report report = (Report) new Report<IndividualTreeBioemissions>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("BioemissionsIndReport");
    }

    private void UVEffectsOfTreesByStrata_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<i_Tree_Eco_v6.Reports.UVEffectsOfTreesByStrata>))
      {
        Report report = (Report) new Report<i_Tree_Eco_v6.Reports.UVEffectsOfTreesByStrata>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("UVEffectsByStrataReport");
    }

    private void WildLifeSuitabilityByPlots_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<i_Tree_Eco_v6.Reports.WildLifeSuitabilityByPlots>)
        return;
      Report report = (Report) new Report<i_Tree_Eco_v6.Reports.WildLifeSuitabilityByPlots>();
      reportViewerForm.Report = report;
    }

    private void WildLifeSuitabilityByStrata_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<i_Tree_Eco_v6.Reports.WildLifeSuitabilityByStrata>)
        return;
      Report report = (Report) new Report<i_Tree_Eco_v6.Reports.WildLifeSuitabilityByStrata>();
      reportViewerForm.Report = report;
    }

    private void rbUVIndexReductionByTreesForUnderTrees_Click(object sender, EventArgs e)
    {
      SimpleChartViewerForm simpleChartViewerForm = this.ShowDocument<SimpleChartViewerForm>(sender);
      if (simpleChartViewerForm == null || simpleChartViewerForm.Chart is UVIndex.UVIndexReductionByTreesForUnderTrees)
        return;
      UVIndex.UVIndexReductionByTreesForUnderTrees treesForUnderTrees = new UVIndex.UVIndexReductionByTreesForUnderTrees();
      simpleChartViewerForm.Chart = (SimpleDateValueChart) treesForUnderTrees;
      this.ShowHelp("UVIndexReductionTreeShadeReport");
    }

    private void rbUVIndexReductionByTreesForOverall_Click(object sender, EventArgs e)
    {
      SimpleChartViewerForm simpleChartViewerForm = this.ShowDocument<SimpleChartViewerForm>(sender);
      if (simpleChartViewerForm == null || simpleChartViewerForm.Chart is UVIndex.UVIndexReductionByTreesForOverall)
        return;
      UVIndex.UVIndexReductionByTreesForOverall byTreesForOverall = new UVIndex.UVIndexReductionByTreesForOverall();
      simpleChartViewerForm.Chart = (SimpleDateValueChart) byTreesForOverall;
      this.ShowHelp("UVIndexReductionOverallReport");
    }

    private void rbUVIndex_Click(object sender, EventArgs e)
    {
      UVChartViewerForm uvChartViewerForm = this.ShowDocument<UVChartViewerForm>(sender);
      if (uvChartViewerForm == null || uvChartViewerForm.Chart is UVIndex.UVIndexData)
        return;
      UVIndex.UVIndexData uvIndexData = new UVIndex.UVIndexData();
      uvChartViewerForm.Chart = (SimpleDateValueChart) uvIndexData;
      this.ShowHelp("UVIndexReport");
    }

    private void rbIsopreneByTrees_Click(object sender, EventArgs e)
    {
      ChartViewerFormForVOC viewerFormForVoc = this.ShowDocument<ChartViewerFormForVOC>(sender);
      if (viewerFormForVoc == null || viewerFormForVoc.Chart is IsopreneByTrees)
        return;
      IsopreneByTrees isopreneByTrees = new IsopreneByTrees();
      viewerFormForVoc.Chart = (VOCDateValueChart) isopreneByTrees;
    }

    private void rbMonoterpeneByTrees_Click(object sender, EventArgs e)
    {
      ChartViewerFormForVOC viewerFormForVoc = this.ShowDocument<ChartViewerFormForVOC>(sender);
      if (viewerFormForVoc == null || viewerFormForVoc.Chart is MonoterpeneByTrees)
        return;
      MonoterpeneByTrees monoterpeneByTrees = new MonoterpeneByTrees();
      viewerFormForVoc.Chart = (VOCDateValueChart) monoterpeneByTrees;
    }

    private void rbIsopreneByShrubs_Click(object sender, EventArgs e)
    {
      ChartViewerFormForVOC viewerFormForVoc = this.ShowDocument<ChartViewerFormForVOC>(sender);
      if (viewerFormForVoc == null || viewerFormForVoc.Chart is IsopreneByShrubs)
        return;
      IsopreneByShrubs isopreneByShrubs = new IsopreneByShrubs();
      viewerFormForVoc.Chart = (VOCDateValueChart) isopreneByShrubs;
    }

    private void rbMonoterpeneByShrubs_Click(object sender, EventArgs e)
    {
      ChartViewerFormForVOC viewerFormForVoc = this.ShowDocument<ChartViewerFormForVOC>(sender);
      if (viewerFormForVoc == null || viewerFormForVoc.Chart is MonoterpeneByShrubs)
        return;
      MonoterpeneByShrubs monoterpeneByShrubs = new MonoterpeneByShrubs();
      viewerFormForVoc.Chart = (VOCDateValueChart) monoterpeneByShrubs;
    }

    private void pestReviewOfTreesButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestReviewofTreesandDetailedPest)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestReviewofTreesandDetailedPest);
      this.ShowHelp("PestReviewOfTreesReport");
    }

    private void primaryPestDetailsOfTreesByStrataButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPrimaryPestDetailsForLanduses)
        ipedReportForm.SetReport(IPEDReports.u4IpedPrimaryPestDetailsForLanduses);
      this.ShowHelp("PrimaryPestDetailStrataReport");
    }

    private void primaryPestSummaryOfTreesByStrataButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPrimaryPestSummaryofTreesforLanduses)
        ipedReportForm.SetReport(IPEDReports.u4IpedPrimaryPestSummaryofTreesforLanduses);
      this.ShowHelp("PrimaryPestSummaryStrataReport");
    }

    private void signSymptomDetailsCompleteBySpeciesButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestSignSymptomDetailsCompletebySpecies)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestSignSymptomDetailsCompletebySpecies);
      this.ShowHelp("PestSignSymptomDetailCompeteSpeciesReport");
    }

    private void signSymptomDetailsCompleteByStrataButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestSignSymptomDetailsCompletebyLanduse)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestSignSymptomDetailsCompletebyLanduse);
      this.ShowHelp("PestSignSymptomDetailCompeteStrataReport");
    }

    private void signSymptomDetailsSummaryBySpeciesButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestSignSymptomDetailsSummarybySpecies)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestSignSymptomDetailsSummarybySpecies);
      this.ShowHelp("PestSignSymptomDetailSummarySpeciesReport");
    }

    private void signSymptomDetailsSummaryByStrataButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestSignSymptomDetailsSummarybyLanduses)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestSignSymptomDetailsSummarybyLanduses);
      this.ShowHelp("PestSignSymptomDetailSummaryStrataReport");
    }

    private void signSymptomOverviewBySpeciesButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestSignSymptomOverviewbySpecies)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestSignSymptomOverviewbySpecies);
      this.ShowHelp("PestSignSymptomOverviewSpeciesReport");
    }

    private void signSymptomOverviewByStrataButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestSignSymptomOverviewbyLanduses)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestSignSymptomOverviewbyLanduses);
      this.ShowHelp("PestSignSymptomOverviewStrataReport");
    }

    private void signSymptomReviewOfTreesButton_Click(object sender, EventArgs e)
    {
      IPEDReportForm ipedReportForm = this.ShowDocument<IPEDReportForm>(sender);
      if (ipedReportForm == null)
        return;
      if (ipedReportForm.IpedReport != IPEDReports.u4IpedPestSignSymptomReviewofTrees)
        ipedReportForm.SetReport(IPEDReports.u4IpedPestSignSymptomReviewofTrees);
      this.ShowHelp("PestSignSymptomReviewOfTreesReport");
    }

    private void rbWeatherAirPollutantConcentrationUGM3_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.AirPollutantConcentrationUGM3), DockState.Document, sender);
      this.ShowHelp("PollutantConcentrationReport");
    }

    private void rbWeatherAirQualImprovementShrub_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.AirQualityImprovementForShrubCover), DockState.Document, sender);
      this.ShowHelp("AQImpShrubReport");
    }

    private void rbWeatherAirQualImprovementTree_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.AirQualityImprovementForTreeCover), DockState.Document, sender);
      this.ShowHelp("AQImpTreeReport");
    }

    private void rbWeatherDryDepShrub_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.DryDepShrub), DockState.Document, sender);
      this.ShowHelp("PollutantFluxShrubReport");
    }

    private void rbWeatherDryDepTree_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.DryDepTree), DockState.Document, sender);
      this.ShowHelp("PollutantFluxTreeReport");
    }

    private void rbWeatherPhotosyntheticallyActiveRadiation_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.PhotosyntheticallyActiveRadiation), DockState.Document, sender);
      this.ShowHelp("PhotosyntheticallyActiveRadiationReport");
    }

    private void rbWeatherRain_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.Rain), DockState.Document, sender);
      this.ShowHelp("RainReport");
    }

    private void rbWeatherTempF_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.TempF), DockState.Document, sender);
      this.ShowHelp("TemperatureReport");
    }

    private void rbWeatherTempK_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.TempK), DockState.Document, sender);
      this.ShowHelp("TemperatureReport");
    }

    private void rbTranspirationByTree_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is TranspirationByTree)
        return;
      TranspirationByTree transpirationByTree = new TranspirationByTree();
      chartViewerForm.Chart = (DateValueChart) transpirationByTree;
      this.ShowHelp("TranspirationTreeReport");
    }

    private void rbTranspirationByShrub_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is TranspirationByShrub)
        return;
      TranspirationByShrub transpirationByShrub = new TranspirationByShrub();
      chartViewerForm.Chart = (DateValueChart) transpirationByShrub;
      this.ShowHelp("TranspirationShrubReport");
    }

    private void rbEvaporationByTrees_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is EvaporationByTrees)
        return;
      EvaporationByTrees evaporationByTrees = new EvaporationByTrees();
      chartViewerForm.Chart = (DateValueChart) evaporationByTrees;
      this.ShowHelp("EvaporationTreeReport");
    }

    private void rbEvaporationByShrubs_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is EvaporationByShrubs)
        return;
      EvaporationByShrubs evaporationByShrubs = new EvaporationByShrubs();
      chartViewerForm.Chart = (DateValueChart) evaporationByShrubs;
      this.ShowHelp("EvaporationShrubReport");
    }

    private void rbWaterInterceptionByTrees_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is WaterInterceptionByTrees)
        return;
      WaterInterceptionByTrees interceptionByTrees = new WaterInterceptionByTrees();
      chartViewerForm.Chart = (DateValueChart) interceptionByTrees;
      this.ShowHelp("WaterInterceptedTreeReport");
    }

    private void rbWaterInterceptionByShrubs_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is WaterInterceptionByShrubs)
        return;
      WaterInterceptionByShrubs interceptionByShrubs = new WaterInterceptionByShrubs();
      chartViewerForm.Chart = (DateValueChart) interceptionByShrubs;
      this.ShowHelp("WaterInterceptedShrubReport");
    }

    private void rbReportsUnits_PressedChanged(object sender, EventArgs e)
    {
      if (this.rbReportsEnglish.Pressed && !this.rbEnglish.Pressed)
        this.rbEnglish.Pressed = true;
      else if (this.rbReportsMetric.Pressed && !this.rbMetric.Pressed)
        this.rbMetric.Pressed = true;
      if (this.rbReportsEnglish.Pressed && !this.rtbForecastEnglish.Pressed)
      {
        this.rtbForecastEnglish.Pressed = true;
      }
      else
      {
        if (!this.rbReportsMetric.Pressed || this.rtbForecastMetric.Pressed)
          return;
        this.rtbForecastMetric.Pressed = true;
      }
    }

    private void rbForecastReportUnits_PressedChanged(object sender, EventArgs e)
    {
      if (this.rtbForecastEnglish.Pressed && !this.rbReportsEnglish.Pressed)
      {
        this.rbReportsEnglish.Pressed = true;
      }
      else
      {
        if (!this.rtbForecastMetric.Pressed || this.rbReportsMetric.Pressed)
          return;
        this.rbReportsMetric.Pressed = true;
      }
    }

    private void rbReportsSpecies_PressedChanged(object sender, EventArgs e)
    {
      if (this.rbReportsSpeciesCN.Pressed && !this.rbSpeciesCN.Pressed)
      {
        this.rbSpeciesCN.Pressed = true;
      }
      else
      {
        if (!this.rbReportsSpeciesSN.Pressed || this.rbSpeciesSN.Pressed)
          return;
        this.rbSpeciesSN.Pressed = true;
      }
    }

    private bool isDefault(Forecast f) => f.NumYears == (short) 30 && (int) f.FrostFreeDays == (int) this._ffDays && f.Mortalities.Count == 3 && f.Replanting.Count == 0 && f.PestEvents.Count == 0 && f.WeatherEvents.Count == 0 && f.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback00_49)).Percent == 3.0 && f.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback50_74)).Percent == 13.08 && f.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback75_99)).Percent == 50.0;

    private Forecast addDefaultForecast()
    {
      Forecast forecast = new Forecast()
      {
        Year = this.m_year,
        Title = this.uniqueTitle(ForecastRes.DefaultStr),
        NumYears = 30,
        FrostFreeDays = this._ffDays,
        Changed = true
      };
      forecast.Mortalities.Add(new Mortality()
      {
        Forecast = forecast,
        Type = ForecastRes.BaseStr,
        Value = ForecastRes.Dieback00_49,
        Percent = 3.0,
        IsPercentStarting = false
      });
      forecast.Mortalities.Add(new Mortality()
      {
        Forecast = forecast,
        Type = ForecastRes.BaseStr,
        Value = ForecastRes.Dieback50_74,
        Percent = 13.08,
        IsPercentStarting = false
      });
      forecast.Mortalities.Add(new Mortality()
      {
        Forecast = forecast,
        Type = ForecastRes.BaseStr,
        Value = ForecastRes.Dieback75_99,
        Percent = 50.0,
        IsPercentStarting = false
      });
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        try
        {
          this.m_forecasts.Add(forecast);
          this.m_session.Save((object) forecast);
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      return forecast;
    }

    private string uniqueTitle(string baseStr)
    {
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        try
        {
          IList<string> stringList = this.m_session.QueryOver<Forecast>().Inner.JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Forecast, Year>>) (f => f.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => y.Guid == this.m_year.Guid)).Select((System.Linq.Expressions.Expression<Func<Forecast, object>>) (fc => fc.Title)).List<string>();
          transaction.Commit();
          int num = 0;
          string str = baseStr;
          while (stringList.Contains(str))
            str = string.Format("{0}_{1}", (object) baseStr, (object) ++num);
          return str;
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public void ResetForecastDefaults()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      Task.Factory.StartNew((System.Action) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          Mortality mortality1 = this.m_forecast.Mortalities.Where<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback00_49)).Single<Mortality>();
          Mortality mortality2 = this.m_forecast.Mortalities.Where<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback50_74)).Single<Mortality>();
          Mortality mortality3 = this.m_forecast.Mortalities.Where<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback75_99)).Single<Mortality>();
          this.m_forecast.NumYears = (short) 30;
          this.m_forecast.FrostFreeDays = this._ffDays;
          mortality1.Percent = 3.0;
          mortality2.Percent = 13.08;
          mortality3.Percent = 50.0;
          this.m_session.SaveOrUpdate((object) this.m_forecast);
          this.m_session.SaveOrUpdate((object) mortality1);
          this.m_session.SaveOrUpdate((object) mortality2);
          this.m_session.SaveOrUpdate((object) mortality3);
          Mortality[] array1 = this.m_forecast.Mortalities.Where<Mortality>((Func<Mortality, bool>) (m => m.Type != ForecastRes.BaseStr)).ToArray<Mortality>();
          Replanting[] array2 = this.m_forecast.Replanting.ToArray<Replanting>();
          foreach (Mortality mortality4 in array1)
          {
            this.m_forecast.Mortalities.Remove(mortality4);
            this.m_session.Delete((object) mortality4);
          }
          foreach (Replanting replanting in array2)
          {
            this.m_forecast.Replanting.Remove(replanting);
            this.m_session.Delete((object) replanting);
          }
          while (this.m_forecast.PestEvents.Count > 0)
          {
            ForecastPestEvent forecastPestEvent = this.m_forecast.PestEvents.First<ForecastPestEvent>();
            this.m_forecast.PestEvents.Remove(forecastPestEvent);
            this.m_session.Delete((object) forecastPestEvent);
          }
          while (this.m_forecast.WeatherEvents.Count > 0)
          {
            ForecastWeatherEvent forecastWeatherEvent = this.m_forecast.WeatherEvents.First<ForecastWeatherEvent>();
            this.m_forecast.WeatherEvents.Remove(forecastWeatherEvent);
            this.m_session.Delete((object) forecastWeatherEvent);
          }
          transaction.Commit();
        }
        EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(this.m_forecast), (Control) this);
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t => this.LoadForecast(this.m_ps.InputSession.ForecastKey.Value)), scheduler);
    }

    private void Mortality_Updated(object sender, EntityUpdated<Mortality> e)
    {
      if (this.m_forecast == null)
        return;
      foreach (Eco.Domain.v6.Entity mortality in (IEnumerable<Mortality>) this.m_forecast.Mortalities)
      {
        if (mortality.Guid == e.Guid)
        {
          this.LoadForecast(this.m_forecast.Guid);
          break;
        }
      }
    }

    private void Forecast_Updated(object sender, EntityUpdated<Forecast> e)
    {
      if (this.m_forecast == null || !(this.m_forecast.Guid == e.Guid))
        return;
      this.LoadForecast(this.m_forecast.Guid);
    }

    private void ConfigurationComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.ConfigurationComboBox.SelectedIndex == -1 || this.m_selectingProject)
        return;
      this.m_ps.InputSession.ForecastKey = new Guid?((Guid) this.ConfigurationComboBox.SelectedItem.Tag);
      if (this.c1Ribbon1.SelectedTab != this.rtForecast)
        return;
      this.ShowHelp("ForecastConfigurations");
      if (this.dockPanel1.ActiveDocument is ForecastContentForm)
        return;
      this.showTabSplash();
    }

    private void BasicInputsButton_Click(object sender, EventArgs e) => this.ShowDocument<ForecastBasicForm>(sender);

    private void MortalityButton_Click(object sender, EventArgs e) => this.ShowDocument<ForecastMortalityForm>(sender);

    private void ReplantingButton_Click(object sender, EventArgs e) => this.ShowDocument<ForecastReplantingForm>(sender);

    private void StormItem_Click(object sender, EventArgs e)
    {
      this.ShowDocument<ForecastWeatherForm>(sender);
      this.ShowHelp("ForecastEventForm");
    }

    private void PestsItem_Click(object sender, EventArgs e) => this.ShowDocument<ForecastPestForm>(sender);

    private void DefaultsButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("ForecastConfigurations");
      if (MessageBox.Show(string.Format(ForecastRes.DefaultForecastWarning, (object) this.ConfigurationComboBox.SelectedItem.Text), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.ResetForecastDefaults();
    }

    private void ForecastRenameButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("ForecastConfigurations");
      ForecastRenameForm forecastRenameForm = new ForecastRenameForm();
      forecastRenameForm.NameTextBox.DataBindings.Add("Text", (object) this.m_forecast, "Title");
      if (forecastRenameForm.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.ConfigurationComboBox.Text = this.m_forecast.Title;
      this.ConfigurationComboBox.SelectedItem.Text = this.m_forecast.Title;
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        this.m_session.Update((object) this.m_forecast);
        transaction.Commit();
      }
      EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(this.m_forecast.Guid), (Control) this);
    }

    private void NewForecastButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("ForecastConfigurations");
      Task.Factory.StartNew<Forecast>((Func<Forecast>) (() => this.addDefaultForecast()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Forecast>>) (t =>
      {
        Forecast result = t.Result;
        RibbonButton ribbonButton = new RibbonButton()
        {
          Text = result.Title,
          Tag = (object) result.Guid
        };
        this.ConfigurationComboBox.Items.Add((RibbonItem) ribbonButton);
        this.ConfigurationComboBox.SelectedItem = ribbonButton;
      }), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void CopyForecastButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("ForecastConfigurations");
      int num = (int) MessageBox.Show(ForecastRes.CopyForecastMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      Task.Factory.StartNew<Forecast>((Func<Forecast>) (() =>
      {
        Forecast forecast = this.m_forecast.Clone() as Forecast;
        forecast.Title = this.uniqueTitle(this.m_forecast.Title + "_" + ForecastRes.CopyStr);
        forecast.Changed = true;
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Save((object) forecast);
          transaction.Commit();
        }
        return forecast;
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Forecast>>) (t =>
      {
        Forecast result = t.Result;
        RibbonButton ribbonButton = new RibbonButton()
        {
          Text = result.Title,
          Tag = (object) result.Guid
        };
        this.ConfigurationComboBox.Items.Add((RibbonItem) ribbonButton);
        this.ConfigurationComboBox.SelectedItem = ribbonButton;
      }), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void DeleteForecastButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("ForecastConfigurations");
      if (this.ConfigurationComboBox.Items.Count == 1)
      {
        int num = (int) MessageBox.Show(ForecastRes.OneForecastError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
      else
      {
        string text = this.ConfigurationComboBox.Text;
        if (MessageBox.Show(string.Format(ForecastRes.DeleteForecastWarning, (object) text), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        this.ConfigurationComboBox.Items.Remove(text);
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) this.m_forecast);
          transaction.Commit();
        }
        this.ConfigurationComboBox.SelectedIndex = 0;
        this.InitForecastOptions();
      }
    }

    private void RunForecastButton_Click(object sender, EventArgs e)
    {
      if (!this.m_forecast.Changed)
      {
        if (MessageBox.Show(string.Format("{0}  {1}", (object) ForecastRes.RerunForecastWarning, (object) ForecastRes.ForecastDurationWarning), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
      }
      else
      {
        int num;
        if (this.m_series.IsSample)
          num = this.m_ps.InputSession.CreateSession().CreateCriteria<Eco.Domain.v6.Tree>().CreateAlias("Plot", "p").CreateAlias("p.Year", "y").Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.m_year.Guid)).SetProjection(Projections.RowCount()).UniqueResult<int>();
        else
          num = this.m_ps.InputSession.CreateSession().CreateCriteria<Eco.Domain.v6.Tree>().CreateAlias("Plot", "p").CreateAlias("p.Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.m_year.Guid)).SetProjection(Projections.RowCount()).UniqueResult<int>();
        if (num > 25000 && MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.WarnForecastNumberOfTrees, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
      }
      ForecastSummaryForm forecastSummaryForm = new ForecastSummaryForm();
      forecastSummaryForm.ForecastSuccessful += new EventHandler<EventArgs>(this.SummaryForm_ForecastSuccessful);
      forecastSummaryForm.ForecastFailed += new EventHandler<EventArgs>(this.summaryForm_ForecastFailed);
      forecastSummaryForm.ForecastCancelled += new EventHandler<EventArgs>(this.summaryForm_ForecastCancelled);
      int num1 = (int) forecastSummaryForm.ShowDialog((IWin32Window) this);
      forecastSummaryForm.Dispose();
    }

    private void SummaryForm_ForecastSuccessful(object sender, EventArgs e)
    {
      this.m_forecast.Changed = false;
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        this.m_session.SaveOrUpdate((object) this.m_forecast);
        transaction.Commit();
      }
      EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(this.m_forecast), (Control) this);
      this.InitForecastOptions();
    }

    private void summaryForm_ForecastFailed(object sender, EventArgs e)
    {
    }

    private void summaryForm_ForecastCancelled(object sender, EventArgs e)
    {
    }

    private void ParametersReportButton_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<ForecastSummary>))
        reportViewerForm.Report = (Report) new Report<ForecastSummary>();
      this.ShowHelp("ForecastParameterSummaryReport");
    }

    private void ForecastEditDataButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("EnableEditing_Forecast");
      if (MessageBox.Show(i_Tree_Eco_v6.Resources.Strings.EnableForecastEditing, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      Task.Factory.StartNew((System.Action) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.GetNamedQuery("DeleteCohorts").SetGuid("guid", this.m_ps.InputSession.ForecastKey.Value).ExecuteUpdate();
          this.m_session.GetNamedQuery("DeletePollutantResults").SetGuid("guid", this.m_ps.InputSession.ForecastKey.Value).ExecuteUpdate();
          this.m_forecast.Changed = true;
          this.m_session.SaveOrUpdate((object) this.m_forecast);
          transaction.Commit();
        }
        EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(this.m_forecast), (Control) this);
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t => this.InitForecastOptions()), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void NumTreesTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastNumTrees>))
        forecastReportForm.Report = (Report) new Report<ForecastNumTrees>();
      this.ShowHelp("ForecastNumberOfTreesReport");
    }

    private void PctCoverTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPctCover>))
        forecastReportForm.Report = (Report) new Report<ForecastPctCover>();
      this.ShowHelp("ForecastPercentTreeCoverReport");
    }

    private void TreeCoverAreaTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastTreeCoverArea>))
        forecastReportForm.Report = (Report) new Report<ForecastTreeCoverArea>();
      this.ShowHelp("ForecastTreeCoverAreaReport");
    }

    private void DbhGrowthTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastDbhGrowth>))
        forecastReportForm.Report = (Report) new Report<ForecastDbhGrowth>();
      this.ShowHelp("ForecastAvgDBHGrowthReport");
    }

    private void DbhDistribTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastDbhDistrib>))
        forecastReportForm.Report = (Report) new Report<ForecastDbhDistrib>();
      this.ShowHelp("ForecastDBHDistributionReport");
    }

    private void LeafAreaTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastLeafArea>))
        forecastReportForm.Report = (Report) new Report<ForecastLeafArea>();
      this.ShowHelp("ForecastTotalLeafAreaReport");
    }

    private void LeafAreaIndexTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastLeafAreaIndex>))
        forecastReportForm.Report = (Report) new Report<ForecastLeafAreaIndex>();
      this.ShowHelp("ForecastLeafAreaIndexReport");
    }

    private void TreeBiomassTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastTreeBiomass>))
        forecastReportForm.Report = (Report) new Report<ForecastTreeBiomass>();
      this.ShowHelp("ForecastTotalTreeBiomassReport");
    }

    private void TotalLfBiomassTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastLeafBiomass>))
        forecastReportForm.Report = (Report) new Report<ForecastLeafBiomass>();
      this.ShowHelp("ForecastTotalLeafBiomassReport");
    }

    private void AreaLfBiomassTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastAreaLeafBiomass>))
      {
        forecastReportForm.Title = "Area Leaf Biomass";
        forecastReportForm.Report = (Report) new Report<ForecastAreaLeafBiomass>();
      }
      this.ShowHelp("ForecastTotalLeafBiomassAreaReport");
    }

    private void BasalAreaTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastBasalArea>))
        forecastReportForm.Report = (Report) new Report<ForecastBasalArea>();
      this.ShowHelp("ForecastTotalBasalAreaReport");
    }

    private void NumTreesByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastNumTreesByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastNumTreesByStrata>();
      this.ShowHelp("ForecastByStrataNumberofTreesReport");
    }

    private void PctCoverByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPctCoverByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastPctCoverByStrata>();
      this.ShowHelp("ForecastByStrataTreeCoverReport");
    }

    private void TreeCoverAreaByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastTreeCoverAreaByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastTreeCoverAreaByStrata>();
      this.ShowHelp("ForecastByStrataTreeCoverAreaReport");
    }

    private void DbhGrowthByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastDbhGrowthByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastDbhGrowthByStrata>();
      this.ShowHelp("ForecastByStrataDBHGrowthReport");
    }

    private void LeafAreaByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastLeafAreaByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastLeafAreaByStrata>();
      this.ShowHelp("ForecastByStrataLeafAreaReport");
    }

    private void LeafAreaIndexByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastLeafAreaIndexByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastLeafAreaIndexByStrata>();
      this.ShowHelp("ForecastByStrataLeafAreaIndexReport");
    }

    private void TreeBiomassByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastTreeBiomassByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastTreeBiomassByStrata>();
      this.ShowHelp("ForecastByStrataTreeBiomassReport");
    }

    private void TotalLfBiomassByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastLeafBiomassByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastLeafBiomassByStrata>();
      this.ShowHelp("ForecastByStrataLeafBiomassReport");
    }

    private void AreaLfBiomassByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastAreaLeafBiomassByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastAreaLeafBiomassByStrata>();
      this.ShowHelp("ForecastByStrataLeafBiomassAreaReport");
    }

    private void BasalAreaByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastBasalAreaByStrata>))
        forecastReportForm.Report = (Report) new Report<ForecastBasalAreaByStrata>();
      this.ShowHelp("ForecastByStrataBasalAreaReport");
    }

    private void ForecastCarbonStorage_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<i_Tree_Eco_v6.Reports.ForecastCarbonStorage>))
        forecastReportForm.Report = (Report) new Report<i_Tree_Eco_v6.Reports.ForecastCarbonStorage>();
      this.ShowHelp("ForecastBenefitsCarbonStorageReport");
    }

    private void ForecastCarbonStorageByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<i_Tree_Eco_v6.Reports.ForecastCarbonStorageByStrata>))
        forecastReportForm.Report = (Report) new Report<i_Tree_Eco_v6.Reports.ForecastCarbonStorageByStrata>();
      this.ShowHelp("ForecastBenefitsCarbonStorageStrataReport");
    }

    private void ForecastCarbonSequestration_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<i_Tree_Eco_v6.Reports.ForecastCarbonSequestration>))
        forecastReportForm.Report = (Report) new Report<i_Tree_Eco_v6.Reports.ForecastCarbonSequestration>();
      this.ShowHelp("ForecastBenefitsCarbonSequestrationReport");
    }

    private void ForecastCarbonSequestrationByStrata_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<i_Tree_Eco_v6.Reports.ForecastCarbonSequestrationByStrata>))
        forecastReportForm.Report = (Report) new Report<i_Tree_Eco_v6.Reports.ForecastCarbonSequestrationByStrata>();
      this.ShowHelp("ForecastBenefitsCarbonSequestrationStrataReport");
    }

    private void AllTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPollutantRemovalAll>))
        forecastReportForm.Report = (Report) new Report<ForecastPollutantRemovalAll>();
      this.ShowHelp("ForecastFunctionalAllReport");
    }

    private void COTotal_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPollutantRemovalCO>))
        forecastReportForm.Report = (Report) new Report<ForecastPollutantRemovalCO>();
      this.ShowHelp("ForecastFunctionalCOReport");
    }

    private void NO2Total_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPollutantRemovalNO2>))
        forecastReportForm.Report = (Report) new Report<ForecastPollutantRemovalNO2>();
      this.ShowHelp("ForecastFunctionalNO2Report");
    }

    private void O3Total_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPollutantRemovalO3>))
        forecastReportForm.Report = (Report) new Report<ForecastPollutantRemovalO3>();
      this.ShowHelp("ForecastFunctionalO3Report");
    }

    private void SO2Total_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPollutantRemovalSO2>))
        forecastReportForm.Report = (Report) new Report<ForecastPollutantRemovalSO2>();
      this.ShowHelp("ForecastFunctionalSO2Report");
    }

    private void PM25Total_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPollutantRemovalPM>))
        forecastReportForm.Report = (Report) new Report<ForecastPollutantRemovalPM>();
      this.ShowHelp("ForecastFunctionalPM25Report");
    }

    private void PM10Total_Click(object sender, EventArgs e)
    {
      ForecastReportForm forecastReportForm = this.ShowDocument<ForecastReportForm>(sender);
      if (forecastReportForm == null)
        return;
      if (!(forecastReportForm.Report is Report<ForecastPollutantRemovalPM10>))
        forecastReportForm.Report = (Report) new Report<ForecastPollutantRemovalPM10>();
      this.ShowHelp("ForecastFunctionalPM10Report");
    }

    private void manualButton_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<UserManualForm>(sender) == null)
        return;
      this.ShowHelp("manualButton");
    }

    private void rbiTreeMethods_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<ITreeMethodsForm>(sender) == null)
        return;
      this.ShowHelp("rtSupport");
    }

    private void videoLearningButton_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<VideoLearningForm>(sender) == null)
        return;
      this.ShowHelp("videoLearningButton");
    }

    private void forumButton_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<UserForumForm>(sender) == null)
        return;
      this.ShowHelp("forumButton");
    }

    private void visitITreeToolsDotOrgButton_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<VisitWebsiteForm>(sender) == null)
        return;
      this.ShowHelp("visitITreeToolsDotOrgButton");
    }

    private void checkForUpdatesButton_Click(object sender, EventArgs e)
    {
      if (!Program.LaunchUpdater((Control) this))
        return;
      this.ShowHelp("checkForUpdatesButton");
    }

    private void aboutButton_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<AboutForm>(sender) == null)
        return;
      this.ShowHelp("aboutButton");
    }

    private void rbtnShowWhatsNew_Click(object sender, EventArgs e) => this.ShowDocument<WhatsNewEmbeddedForm>(sender);

    private void feedbackButton_Click(object sender, EventArgs e) => this.ShowDocument<FeedbackForm>(sender);

    private void rbExportCSV_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument1 && !activeDocument1.Validate() || !(this.dockPanel1.ActiveDocument is IExportable activeDocument2) || !activeDocument2.CanExport(ExportFormat.CSV))
        return;
      this.CSVDialog(activeDocument2);
    }

    public void CSVDialog(IExportable exp)
    {
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.AddExtension = true;
        saveFileDialog.OverwritePrompt = true;
        saveFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterCSV, (object) Settings.Default.ExtCSV),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
        });
        saveFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.ExportCSV;
        saveFileDialog.ValidateNames = true;
        saveFileDialog.ShowHelp = false;
        saveFileDialog.CreatePrompt = false;
        if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        try
        {
          exp.Export(ExportFormat.CSV, saveFileDialog.FileName);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrExport, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private void rbDataExportKML_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument1 && !activeDocument1.Validate() || !(this.dockPanel1.ActiveDocument is IExportable activeDocument2) || !activeDocument2.CanExport(ExportFormat.KML))
        return;
      this.KMLDialog(activeDocument2);
    }

    public void KMLDialog(IExportable exp)
    {
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.AddExtension = true;
        saveFileDialog.OverwritePrompt = true;
        saveFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterKML, (object) string.Join(";", Settings.Default.ExtKML)),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
        });
        saveFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.ExportKML;
        saveFileDialog.ValidateNames = true;
        saveFileDialog.ShowHelp = false;
        saveFileDialog.CreatePrompt = false;
        if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        try
        {
          exp.Export(ExportFormat.KML, saveFileDialog.FileName);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrExport, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private void rbImport_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IImport activeDocument) || !((ContainerControl) this.dockPanel1.ActiveDocument).Validate())
        return;
      this.ShowHelp("ImportDataForm");
      using (ImportDataForm importDataForm = new ImportDataForm(activeDocument.ImportSpec()))
      {
        if (importDataForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        ProcessingForm processingForm = new ProcessingForm();
        Progress<ProgressEventArgs> progress1 = new Progress<ProgressEventArgs>();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Task task = activeDocument.ImportData(importDataForm.ImportedData, (IProgress<ProgressEventArgs>) progress1, cancellationTokenSource.Token);
        Task t = task;
        Progress<ProgressEventArgs> progress2 = progress1;
        CancellationTokenSource token = cancellationTokenSource;
        int num1 = (int) processingForm.ShowDialog((IWin32Window) this, t, progress2, token);
        if (task.IsCanceled || !task.IsFaulted)
          return;
        int num2 = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrImport, (object) task.Exception.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void mnuFileOpenSampleProject_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate())
        return;
      OpenSampleProject openSampleProject = new OpenSampleProject();
      if (openSampleProject.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string fileName = openSampleProject.FileName;
      if (!File.Exists(fileName) || !FileSignature.IsSqliteDatabase(fileName))
        return;
      this.m_showMetadataReport = true;
      string str1 = Path.Combine(Application.UserAppDataPath, Path.GetFileNameWithoutExtension(fileName));
      int num = 0;
      string str2;
      do
      {
        str2 = "." + num.ToString() + ".ieco";
        ++num;
      }
      while (File.Exists(str1 + str2));
      string str3 = str1 + str2;
      File.Copy(fileName, str3, true);
      this.OpenProjectFile(str3, true);
    }

    private void rmbModelProcessingNotes_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<ModelProcessingNotes>))
        reportViewerForm.Report = (Report) new Report<ModelProcessingNotes>();
      this.ShowHelp("ModelNotesReport");
    }

    private void ReplacementValueByDBHClassAndSpecies_Click(object sender, EventArgs e) => this.ShowHelp("StructuralValueDBHSpeciesReport");

    private void rbMDDCompositionOfTreesByStratum_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<MeasuredTreeDetailsByStrata>))
        reportViewerForm.Report = (Report) new Report<MeasuredTreeDetailsByStrata>();
      this.ShowHelp("InventorySummaryStrataReport");
    }

    private void FoodscapeBenefitsOfTreesBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<FoodscapeBenefitsOfTreesBySpecies>))
        reportViewerForm.Report = (Report) new Report<FoodscapeBenefitsOfTreesBySpecies>();
      this.ShowHelp("FoodscapeBenefitsOfTreesBySpeciesReport");
    }

    private void LeafNutrientsofTreesMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<LeafNutrientsOfTreesBySpecies>))
      {
        Report report = (Report) new Report<LeafNutrientsOfTreesBySpecies>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("LeafNutrientsBySpeciesReport");
    }

    private void rbMDDTreeBenefitsCarbonSequestration_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeCarbonSequestration>))
        reportViewerForm.Report = (Report) new Report<IndividualTreeCarbonSequestration>();
      this.ShowHelp("CarbonSequestrationIndReport");
    }

    private void rbMDDTreeBenefitsCarbonStorage_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeCarbonStorage>))
        reportViewerForm.Report = (Report) new Report<IndividualTreeCarbonStorage>();
      this.ShowHelp("CarbonStorageIndReport");
    }

    private void rbMaintTaskReport_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<MaintTaskLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<MaintTaskLookupReport>();
    }

    private void rbMaintRecReport_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<MaintRecLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<MaintRecLookupReport>();
    }

    private void rbMaintTaskBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<MaintTaskTotalsLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<MaintTaskTotalsLookupReport>();
    }

    private void rbSidewalkConflictsReport_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<SidewalkLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<SidewalkLookupReport>();
    }

    private void rbUtilityConflictsReport_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<WireConflictLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<WireConflictLookupReport>();
    }

    private void rbFieldOneReport_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<OtherOneLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<OtherOneLookupReport>();
    }

    private void rbFieldTwoReport_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<OtherTwoLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<OtherTwoLookupReport>();
    }

    private void rbFieldThreeReport_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<OtherThreeLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<OtherThreeLookupReport>();
    }

    private void rbRelativePerformanceIndexBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<RelativePerformanceIndex>))
        reportViewerForm.Report = (Report) new Report<RelativePerformanceIndex>();
      this.ShowHelp("RelativePerformanceIndexSpeciesReport");
    }

    private void SusceptibilityOfTreesToPestsByStrata_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<i_Tree_Eco_v6.Reports.SusceptibilityOfTreesToPestsByStrata>))
        reportViewerForm.Report = (Report) new Report<i_Tree_Eco_v6.Reports.SusceptibilityOfTreesToPestsByStrata>();
      this.ShowHelp("SusceptibilityPestsStrataReport");
    }

    private void rbAnnualCosts_Click(object sender, EventArgs e) => this.ShowDocument<AnnualCostsForm>(sender);

    private void rbManagementCostsByExpenditure_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<ManagementCosts>))
        reportViewerForm.Report = (Report) new Report<ManagementCosts>();
      this.ShowHelp("ManagementCostsExpenditureReport");
    }

    private void rbNetAnnualBenefitsForAllTreesSample_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<NetAnnualBenefits>))
        reportViewerForm.Report = (Report) new Report<NetAnnualBenefits>();
      this.ShowHelp("NetAnnualBenefitsReport");
    }

    private void rbPublicPrivateByStrata_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<PublicPrivate>))
        reportViewerForm.Report = (Report) new Report<PublicPrivate>();
      this.ShowHelp("PopSumPublicPrivateStrataReport");
    }

    private void rbStreetTreesByStrata_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<StreetTrees>))
        reportViewerForm.Report = (Report) new Report<StreetTrees>();
      this.ShowHelp("PopSumStreetTreesStrataReport");
    }

    private async void SendDataButton_Click(object sender, EventArgs e)
    {
      MainRibbonForm mainRibbonForm = this;
      mainRibbonForm.ShowHelp("SendDatatoForestService");
      EulaForm eulaForm = new EulaForm();
      int num1 = (int) eulaForm.ShowDialog((IWin32Window) mainRibbonForm);
      frmDataValidationSignal aValidationLabel;
      if (eulaForm.DialogResult != DialogResult.OK)
      {
        mainRibbonForm.showTabSplash();
        aValidationLabel = (frmDataValidationSignal) null;
      }
      else
      {
        frmDataValidationSignal validationSignal = new frmDataValidationSignal();
        validationSignal.StartPosition = FormStartPosition.Manual;
        aValidationLabel = validationSignal;
        aValidationLabel.Location = new Point(mainRibbonForm.Left + mainRibbonForm.Width / 2 - aValidationLabel.Width / 2, mainRibbonForm.Top + mainRibbonForm.Height / 2 - aValidationLabel.Height / 2);
        aValidationLabel.ShowWithParentFormLock((Form) mainRibbonForm);
        bool dataIsValid = true;
        try
        {
          dataIsValid = true;
          Dictionary<string, string> treeSynonymCultivar = new Dictionary<string, string>();
          Dictionary<string, string> shrubSynonymCultivar = new Dictionary<string, string>();
          Dictionary<string, string> mortalityTreeSynonymCultivar = new Dictionary<string, string>();
          await Task.Factory.StartNew((System.Action) (() =>
          {
            using (SASProcessor sasProcessor = new SASProcessor())
            {
              sasProcessor.prepareToStart(this.m_ps, (Form) this);
              dataIsValid = sasProcessor.DataValidationOfSynonymOrCultivar(false, treeSynonymCultivar, shrubSynonymCultivar, mortalityTreeSynonymCultivar);
            }
          }), CancellationToken.None, TaskCreationOptions.None, mainRibbonForm.m_ps.Scheduler);
          if (!dataIsValid)
          {
            SynonymOrCultivarForm synonymOrCultivarForm = new SynonymOrCultivarForm();
            synonymOrCultivarForm.InitializeForm(mainRibbonForm.m_ps, false, treeSynonymCultivar, shrubSynonymCultivar, mortalityTreeSynonymCultivar);
            int num2 = (int) synonymOrCultivarForm.ShowDialog((IWin32Window) mainRibbonForm);
          }
          dataIsValid = true;
          string fatalError = "";
          string warningError = "";
          await Task.Factory.StartNew((System.Action) (() =>
          {
            using (SASProcessor sasProcessor = new SASProcessor())
            {
              sasProcessor.prepareToStart(this.m_ps, (Form) this);
              dataIsValid = sasProcessor.DataValidation(sasProcessor.workingInputDatabaseName, ref fatalError, ref warningError);
            }
          }), CancellationToken.None, TaskCreationOptions.None, mainRibbonForm.m_ps.Scheduler);
          if (!dataIsValid)
          {
            string fromLoggedFileOrErrorMessage = SASResources.DataValidationErrors + "\r\n\r\n" + fatalError;
            if (warningError.Length > 0)
              fromLoggedFileOrErrorMessage = fromLoggedFileOrErrorMessage + "\r\n\r\n" + SASResources.DataValidationWarnings + "\r\n\r\n" + warningError;
            LoggedErrorForm loggedErrorForm = new LoggedErrorForm();
            loggedErrorForm.initializeForm(fromLoggedFileOrErrorMessage, false);
            int num3 = (int) loggedErrorForm.ShowDialog((IWin32Window) mainRibbonForm);
          }
          if (dataIsValid)
          {
            if (!string.IsNullOrEmpty(warningError))
            {
              string msg = SASResources.DataValidationWarnings + "\r\n\r\n" + warningError;
              LoggedErrorForm loggedErrorForm = new LoggedErrorForm();
              loggedErrorForm.InitForContinue(msg);
              int num4 = (int) loggedErrorForm.ShowDialog((IWin32Window) mainRibbonForm);
              loggedErrorForm.Close();
            }
          }
        }
        catch (Exception ex)
        {
          int num5 = (int) MessageBox.Show(ex.Message, i_Tree_Eco_v6.Resources.Strings.DataValidationError, MessageBoxButtons.OK);
          aValidationLabel.Close();
          aValidationLabel = (frmDataValidationSignal) null;
          return;
        }
        aValidationLabel.Close();
        if (!dataIsValid)
        {
          aValidationLabel = (frmDataValidationSignal) null;
        }
        else
        {
          frmSendToSAS frmSendToSas = new frmSendToSAS();
          frmSendToSas.labelProjectInfo.Text = mainRibbonForm.GetProjectInfoLabel();
          frmSendToSas.InitializeForm(mainRibbonForm.m_ps, (Form) mainRibbonForm);
          aValidationLabel = (frmDataValidationSignal) null;
        }
      }
    }

    private void RetrieveResultsButton_Click(object sender, EventArgs e)
    {
      this.ShowHelp("RetrieveResults");
      frmRetrieveFromSAS frmRetrieveFromSas = new frmRetrieveFromSAS();
      frmRetrieveFromSas.labelProjectInfo.Text = this.GetProjectInfoLabel();
      if (frmRetrieveFromSas.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.CloseAllDocuments();
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>((object) this.projectMetadataButton);
      Report report = (Report) new Report<ProjectMetadata>();
      reportViewerForm.Title = "Project Metadata";
      reportViewerForm.Report = report;
      this.ShowHelp("ProjectMetadataReport");
    }

    public string GetProjectInfoLabel() => this.Text = string.Format("({0}, {1}, {2})", (object) this.m_project.Name, (object) this.m_series.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.m_year.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    private void mnuFileSaveProjectAs_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate() || this.m_ps == null || this.m_ps.InputSession == null || this.m_ps.InputSession.InputDb == null)
        return;
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Title = i_Tree_Eco_v6.Resources.Strings.SaveProjectAs;
      saveFileDialog1.Filter = string.Join("|", new string[2]
      {
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterEcoFile, (object) string.Join(";", Settings.Default.ExtEcoProj)),
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
      });
      saveFileDialog1.CheckPathExists = true;
      saveFileDialog1.OverwritePrompt = true;
      saveFileDialog1.AddExtension = true;
      saveFileDialog1.DefaultExt = "ieco";
      saveFileDialog1.ShowHelp = false;
      saveFileDialog1.CreatePrompt = false;
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      if (saveFileDialog2.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      try
      {
        File.Copy(this.m_ps.InputSession.InputDb, saveFileDialog2.FileName, true);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private async void rbValidate_Click(object sender, EventArgs e)
    {
      MainRibbonForm mainRibbonForm = this;
      mainRibbonForm.ShowHelp("CheckData");
      frmDataValidationSignal aValidationLabel;
      if (mainRibbonForm.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate())
      {
        aValidationLabel = (frmDataValidationSignal) null;
      }
      else
      {
        aValidationLabel = new frmDataValidationSignal();
        aValidationLabel.StartPosition = FormStartPosition.Manual;
        aValidationLabel.Location = new Point(mainRibbonForm.Left + mainRibbonForm.Width / 2 - aValidationLabel.Width / 2, mainRibbonForm.Top + mainRibbonForm.Height / 2 - aValidationLabel.Height / 2);
        aValidationLabel.ShowWithParentFormLock((Form) mainRibbonForm);
        bool validateStatus = true;
        try
        {
          validateStatus = true;
          Dictionary<string, string> treeSynonymCultivar = new Dictionary<string, string>();
          Dictionary<string, string> shrubSynonymCultivar = new Dictionary<string, string>();
          Dictionary<string, string> mortalityTreeSynonymCultivar = new Dictionary<string, string>();
          await Task.Factory.StartNew((System.Action) (() =>
          {
            using (SASProcessor sasProcessor = new SASProcessor())
            {
              sasProcessor.prepareToStart(this.m_ps, (Form) this);
              validateStatus = sasProcessor.DataValidationOfSynonymOrCultivar(true, treeSynonymCultivar, shrubSynonymCultivar, mortalityTreeSynonymCultivar);
            }
          }), CancellationToken.None, TaskCreationOptions.None, mainRibbonForm.m_ps.Scheduler);
          if (!validateStatus)
          {
            SynonymOrCultivarForm synonymOrCultivarForm = new SynonymOrCultivarForm();
            synonymOrCultivarForm.InitializeForm(mainRibbonForm.m_ps, true, treeSynonymCultivar, shrubSynonymCultivar, mortalityTreeSynonymCultivar);
            int num = (int) synonymOrCultivarForm.ShowDialog((IWin32Window) mainRibbonForm);
          }
          validateStatus = true;
          string fatalError = "";
          string warningError = "";
          await Task.Factory.StartNew((System.Action) (() =>
          {
            using (SASProcessor sasProcessor = new SASProcessor())
            {
              sasProcessor.prepareToStart(this.m_ps, (Form) this);
              validateStatus = sasProcessor.DataValidation(sasProcessor.workingInputDatabaseName, ref fatalError, ref warningError);
            }
          }), CancellationToken.None, TaskCreationOptions.None, mainRibbonForm.m_ps.Scheduler);
          if (!validateStatus)
          {
            string fromLoggedFileOrErrorMessage = SASResources.DataValidationErrors + "\r\n\r\n" + fatalError;
            if (warningError.Length > 0)
              fromLoggedFileOrErrorMessage = fromLoggedFileOrErrorMessage + "\r\n\r\n" + SASResources.DataValidationWarnings + "\r\n\r\n" + warningError;
            LoggedErrorForm loggedErrorForm = new LoggedErrorForm();
            loggedErrorForm.initializeForm(fromLoggedFileOrErrorMessage, false);
            int num = (int) loggedErrorForm.ShowDialog((IWin32Window) mainRibbonForm);
          }
          else if (warningError.Length > 0)
          {
            string fromLoggedFileOrErrorMessage = SASResources.DataValidationWarnings + "\r\n\r\n" + warningError;
            LoggedErrorForm loggedErrorForm = new LoggedErrorForm();
            loggedErrorForm.initializeForm(fromLoggedFileOrErrorMessage, false);
            int num = (int) loggedErrorForm.ShowDialog((IWin32Window) mainRibbonForm);
          }
          else
          {
            int num1 = (int) MessageBox.Show((IWin32Window) mainRibbonForm, SASResources.DataValidationSuccess, Application.ProductName, MessageBoxButtons.OK);
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) mainRibbonForm, ex.Message, i_Tree_Eco_v6.Resources.Strings.DataValidationError, MessageBoxButtons.OK);
        }
        aValidationLabel.Close();
        aValidationLabel = (frmDataValidationSignal) null;
      }
    }

    private void mnuFilePackProject_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate() || this.m_ps == null || this.m_ps.InputSession == null)
        return;
      if (this.m_ps.InputSession.InputDb == null)
        return;
      try
      {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
          saveFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.SavePackedProjectAs;
          saveFileDialog.Filter = string.Join("|", new string[2]
          {
            string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterZip, (object) string.Join(";", Settings.Default.ExtZip)),
            string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
          });
          saveFileDialog.CheckPathExists = true;
          saveFileDialog.OverwritePrompt = true;
          saveFileDialog.AddExtension = true;
          saveFileDialog.DefaultExt = Regex.Replace(Settings.Default.ExtZip[0], "^\\*?\\.?", "");
          saveFileDialog.ShowHelp = false;
          saveFileDialog.CreatePrompt = false;
          if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          if (File.Exists(saveFileDialog.FileName))
            File.Delete(saveFileDialog.FileName);
          using (TempFile tempFile = new TempFile(false, "mdb"))
          {
            File.Copy(this.m_ps.InputSession.InputDb, tempFile.name, true);
            using (C1ZipFile c1ZipFile = new C1ZipFile())
            {
              c1ZipFile.Create(saveFileDialog.FileName);
              c1ZipFile.Entries.Add(tempFile.name, Path.GetFileName(this.m_ps.InputSession.InputDb));
              c1ZipFile.Close();
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrPackProject, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void rbEula_Click(object sender, EventArgs e)
    {
      this.ShowDocument<EulaForm>(sender);
      this.ShowHelp("LicenseAgreementButton");
    }

    private void rbStructureSummaryBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<StructureSummaryBySpecies>))
        reportViewerForm.Report = (Report) new Report<StructureSummaryBySpecies>();
      this.ShowHelp("StructureSummarySpeciesReport");
    }

    private void rbStructureSummaryByStrataSpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<StructureSummaryByStrataSpecies>))
        reportViewerForm.Report = (Report) new Report<StructureSummaryByStrataSpecies>();
      this.ShowHelp("StructureSummaryStrataSpeciesReport");
    }

    private void rbBenefitsSummaryBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<BenefitsSummaryBySpecies>))
        reportViewerForm.Report = (Report) new Report<BenefitsSummaryBySpecies>();
      this.ShowHelp("BenefitsSummarySpeciesReport");
    }

    private void rbBenefitsSummaryByStrataSpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<BenefitsSummaryByStrataSpecies>))
        reportViewerForm.Report = (Report) new Report<BenefitsSummaryByStrataSpecies>();
      this.ShowHelp("BenefitsSummaryStrataSpeciesReport");
    }

    private void rbUserGuidesInstall_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<InstallingEcoGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesDataLimitations_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<DataLimitationsGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesExampleProjects_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<ExampleProjectsGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesFieldManual_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<FieldManualGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesInternational_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<InternationalProjectsGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesCapturingCoordinateData_Click(object sender, EventArgs e) => this.ShowDocument<CollectingCoordinateDataGuideForm>(sender);

    private void rbUserGuidesPoststratified_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<PoststratifiedSamplesGuideFrom>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesPrestratified_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<PrestratifiedSamplesGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesStratifyInventory_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<StratifyingInventoryGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesUnstratified_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<UnstratifiedSamplesGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesUsingForecast_Click(object sender, EventArgs e)
    {
      if (this.ShowDocument<UsingForecastGuideForm>(sender) == null)
        return;
      this.ShowHelp("UserGuidesButton");
    }

    private void rbUserGuidesDifferences_v5_v6_Click(object sender, EventArgs e) => this.ShowDocument<Differences_v5v6GuideForm>(sender);

    private void rbUserGuidesInventoryImporter_Click(object sender, EventArgs e) => this.ShowDocument<InventoryImporterGuideForm>(sender);

    private void rcbMinimizeHelp_CheckedChanged(object sender, EventArgs e)
    {
      this.m_ps.MinimizeHelpPanel = this.rcbMinimizeHelp.Checked;
      this.m_frmHelp.DockState = this.rcbMinimizeHelp.Checked ? DockState.DockLeftAutoHide : DockState.DockLeft;
    }

    private void rbExportToCSV_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is IExportable activeDocument) || !activeDocument.CanExport(ExportFormat.CSV))
        return;
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.AddExtension = true;
      saveFileDialog.OverwritePrompt = true;
      saveFileDialog.Filter = "Comma Separated Files (*.csv)|*.csv|All Files (*.*)|*.*";
      saveFileDialog.Title = "Export to CSV";
      saveFileDialog.ValidateNames = true;
      saveFileDialog.ShowHelp = false;
      saveFileDialog.CreatePrompt = false;
      if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      try
      {
        activeDocument.Export(ExportFormat.CSV, saveFileDialog.FileName);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "The following error occurred while exporting your data to CSV:\n\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void rbSpeciesList_Click(object sender, EventArgs e) => this.ShowDocument<SpeciesListForm>(sender);

    private void carbonStorageOfTreesBySpeciesChartMenuItem_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<CarbonStorageOfTreesBySpecies>))
        reportViewerForm.Report = (Report) new Report<CarbonStorageOfTreesBySpecies>();
      this.ShowHelp("CarbonStorageSpeciesReport");
    }

    public void NetAnnualCarbonSequestrationOfTreesBySpeciesMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<NetCarbonSequestrationBySpecies>))
        reportViewerForm.Report = (Report) new Report<NetCarbonSequestrationBySpecies>();
      this.ShowHelp("NetCarbonSequestrationSpeciesReport");
    }

    private void AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<NetCarbonSequestrationByStrata>))
        reportViewerForm.Report = (Report) new Report<NetCarbonSequestrationByStrata>();
      this.ShowHelp("NetCarbonSequestrationStrataReport");
    }

    private void AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem_Click(
      object sender,
      EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<NetCarbonSequestrationByStrataPerUnitArea>))
        reportViewerForm.Report = (Report) new Report<NetCarbonSequestrationByStrataPerUnitArea>();
      this.ShowHelp("NetCarbonSequestrationAreaReport");
    }

    private void rbReportExport_Click(object sender, EventArgs e)
    {
      int num = (int) new ReportExportForm().ShowDialog();
    }

    private void rbAvoidedRunoffByTrees_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is AvoidedRunoffByTrees)
        return;
      AvoidedRunoffByTrees avoidedRunoffByTrees = new AvoidedRunoffByTrees();
      chartViewerForm.Chart = (DateValueChart) avoidedRunoffByTrees;
      this.ShowHelp("AvoidedRunoffTreeReport");
    }

    private void rbWeatherDryDepGrass_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.DryDepGrass), DockState.Document, sender);
      this.ShowHelp("PollutantFluxGrassReport");
    }

    private void rbWeatherAirQualImprovementGrass_Click(object sender, EventArgs e)
    {
      if (!this.CloseAllDocuments())
        return;
      this.ShowDocument((DockContent) new WeatherChart(WeatherReport.AirQualityImprovementForGrassCover), DockState.Document, sender);
      this.ShowHelp("AQImpGrassReport");
    }

    private void airQualityHealthImpactsandValuesbyShrubs_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<AirQualityHealthImpactsAndValuesbyShrubs>)
        return;
      Report report = (Report) new Report<AirQualityHealthImpactsAndValuesbyShrubs>();
      reportViewerForm.Report = report;
    }

    private void airQualityHealthImpactsandValuesbyGrass_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<AirQualityHealthImpactsAndValuesbyGrass>)
        return;
      Report report = (Report) new Report<AirQualityHealthImpactsAndValuesbyGrass>();
      reportViewerForm.Report = report;
    }

    private void airQualityHealthImpactsandValuesCombined_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<AirQualityHealthImpactsAndValuesCombined>)
        return;
      Report report = (Report) new Report<AirQualityHealthImpactsAndValuesCombined>();
      reportViewerForm.Report = report;
    }

    private void rbMDDMiscTreeComments_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<TreeComments>)
        return;
      Report report = (Report) new Report<TreeComments>();
      reportViewerForm.Report = report;
    }

    private void rbMDDMiscShrubComments_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<ShrubComments>)
        return;
      Report report = (Report) new Report<ShrubComments>();
      reportViewerForm.Report = report;
    }

    private void rbMDDMiscPlotComments_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<PlotComments>)
        return;
      Report report = (Report) new Report<PlotComments>();
      reportViewerForm.Report = report;
    }

    private void rbCheckBoxGPS_CheckedChanged(object sender, EventArgs e)
    {
      this.m_ps.ShowGPS = this.rbCheckBoxGPS.Checked;
      if (!(this.dockPanel1.ActiveDocument is ReportViewerForm activeDocument) || !activeDocument.Report.hasCoordinates)
        return;
      activeDocument.RefreshReport();
    }

    private void rbCheckBoxComments_CheckedChanged(object sender, EventArgs e)
    {
      this.m_ps.ShowComments = this.rbCheckBoxComments.Checked;
      if (!(this.dockPanel1.ActiveDocument is ReportViewerForm activeDocument) || !activeDocument.Report.hasComments)
        return;
      activeDocument.RefreshReport();
    }

    private void rbCheckBoxHudeZeros_CheckedChanged(object sender, EventArgs e)
    {
      this.m_ps.HideZeros = this.rbCheckBoxHideZeros.Checked;
      if (!(this.dockPanel1.ActiveDocument is ReportViewerForm activeDocument) || !activeDocument.Report.hasZeros)
        return;
      activeDocument.RefreshReport();
    }

    private void rbMapData_Click(object sender, EventArgs e)
    {
      if (!(this.dockPanel1.ActiveDocument is ReportViewerForm activeDocument) || !activeDocument.Report.hasCoordinates)
        return;
      activeDocument.AppBusy((Form) this, i_Tree_Eco_v6.Resources.Strings.MsgGeneratingPoints);
      try
      {
        Form form = (Form) new MapForm(activeDocument);
        if (this.currMap != null)
          this.currMap.Close();
        this.currMap = form;
        form.Show();
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        string oops = i_Tree_Eco_v6.Resources.Strings.Oops;
        MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
        string caption = oops;
        int buttons = (int) messageBoxButtons;
        int num = (int) MessageBox.Show(message, caption, (MessageBoxButtons) buttons);
      }
      activeDocument.AppReady();
    }

    private void rbCheckBoxUID_CheckedChanged(object sender, EventArgs e)
    {
      this.m_ps.ShowUID = this.rbCheckBoxUID.Checked;
      if (!(this.dockPanel1.ActiveDocument is ReportViewerForm activeDocument) || !activeDocument.Report.hasUID)
        return;
      activeDocument.RefreshReport();
    }

    private void rbMaintRecBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<MaintRecTotalsLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<MaintRecTotalsLookupReport>();
    }

    private void rbSidewalkConflictsBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<SidewalkTotalsLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<SidewalkTotalsLookupReport>();
    }

    private void rbUtilityConflictsBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<WireConflictTotalsLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<WireConflictTotalsLookupReport>();
    }

    private void rbFieldOneBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<OtherOneTotalsLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<OtherOneTotalsLookupReport>();
    }

    private void rbFieldTwoBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<OtherTwoTotalsLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<OtherTwoTotalsLookupReport>();
    }

    private void rbFieldThreeBySpecies_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<OtherThreeTotalsLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<OtherThreeTotalsLookupReport>();
    }

    private void rbMDDCompositionOfPlots_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<AllPlots>)
        return;
      reportViewerForm.Report = (Report) new Report<AllPlots>();
    }

    private void rbMaintRec_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<InvMaintRecLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<InvMaintRecLookupReport>();
    }

    private void rbMaintTask_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<InvMaintTaskLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<InvMaintTaskLookupReport>();
    }

    private void rbSidewalkConflicts_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<InvSidewalkLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<InvSidewalkLookupReport>();
    }

    private void rbMaintUtilityConflicts_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<InvWireConflictLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<InvWireConflictLookupReport>();
    }

    private void rbFieldOne_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<InvOtherOneLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<InvOtherOneLookupReport>();
    }

    private void rbFieldTwo_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<InvOtherTwoLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<InvOtherTwoLookupReport>();
    }

    private void rbFieldThree_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null || reportViewerForm.Report is Report<InvOtherThreeLookupReport>)
        return;
      reportViewerForm.Report = (Report) new Report<InvOtherThreeLookupReport>();
    }

    private void rbAvoidedRunoffByShrubs_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is AvoidedRunoffByShrubs)
        return;
      AvoidedRunoffByShrubs avoidedRunoffByShrubs = new AvoidedRunoffByShrubs();
      chartViewerForm.Chart = (DateValueChart) avoidedRunoffByShrubs;
      this.ShowHelp("AvoidedRunoffShrubReport");
    }

    private void rbPotentialEvaporationByTrees_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is PotentialEvaporationByTrees)
        return;
      PotentialEvaporationByTrees evaporationByTrees = new PotentialEvaporationByTrees();
      chartViewerForm.Chart = (DateValueChart) evaporationByTrees;
      this.ShowHelp("PotentialEvapotranspirationTreeReport");
    }

    private void rbPotentialEvaporationByShrubs_Click(object sender, EventArgs e)
    {
      ChartViewerForm chartViewerForm = this.ShowDocument<ChartViewerForm>(sender);
      if (chartViewerForm == null || chartViewerForm.Chart is PotentialEvaporationByShrubs)
        return;
      PotentialEvaporationByShrubs evaporationByShrubs = new PotentialEvaporationByShrubs();
      chartViewerForm.Chart = (DateValueChart) evaporationByShrubs;
      this.ShowHelp("PotentialEvapotranspirationShrubReport");
    }

    private void rbtnShowChangeLog_Click(object sender, EventArgs e) => this.ShowDocument<ChangelogForm>(sender);

    private void rbReportsExportKML_Click(object sender, EventArgs e) => this.KMLDialog((IExportable) (this.dockPanel1.ActiveDocument as ReportViewerForm).Report);

    private void rbReportsExportCSV_Click(object sender, EventArgs e) => this.CSVDialog((IExportable) (this.dockPanel1.ActiveDocument as ReportViewerForm).Report);

    private void rbDataReferenceObjects_Click(object sender, EventArgs e) => this.ShowDocument<ReferenceObjectsForm>(sender);

    private void rbDataGroundCovers_Click(object sender, EventArgs e) => this.ShowDocument<GroundCoversDataForm>(sender);

    private void rbDataLandUses_Click(object sender, EventArgs e) => this.ShowDocument<LandUsesDataForm>(sender);

    private void rbMDDWoodProducts_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<IndividualTreeWoodProducts>))
      {
        Report report = (Report) new Report<IndividualTreeWoodProducts>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("WoodProductsOfIndividualTreesReport");
    }

    private void AllergyIndexOfTreesByStrata_Click(object sender, EventArgs e)
    {
      ReportViewerForm reportViewerForm = this.ShowDocument<ReportViewerForm>(sender);
      if (reportViewerForm == null)
        return;
      if (!(reportViewerForm.Report is Report<SpeciesAllergyIndexByStratum>))
      {
        Report report = (Report) new Report<SpeciesAllergyIndexByStratum>();
        reportViewerForm.Report = report;
      }
      this.ShowHelp("AllergyIndexOfTreesByStrataumReport");
    }

    private void rbHealthClasses_Click(object sender, EventArgs e) => this.ShowDocument<HealthRptClassesForm>(sender);

    private void rbDBH_Click(object sender, EventArgs e) => this.ShowDocument<DBHsForm>(sender);

    private void mnuFileCreateReinventory_Click(object sender, EventArgs e)
    {
      if (this.dockPanel1.ActiveDocument is Form activeDocument && !activeDocument.Validate())
        return;
      ReinventoryWizard reinventoryWizard = new ReinventoryWizard();
      if (reinventoryWizard.ShowDialog((IWin32Window) this) != DialogResult.OK || !reinventoryWizard.OpenOnceCompleted || !this.CloseAllDocuments())
        return;
      this.OpenProjectFile(reinventoryWizard.ProjectLocation);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainRibbonForm));
      this.statusTrackBar = new RibbonTrackBar();
      this.viewsMenu = new RibbonMenu();
      this.sbPrintLayoutToggleButton = new RibbonToggleButton();
      this.sbFullScreenToggleButton = new RibbonToggleButton();
      this.sbWebLayoutToggleButton = new RibbonToggleButton();
      this.sbProectInfoLabel = new RibbonLabel();
      this.ribbonSeparator13 = new RibbonSeparator();
      this.c1StatusBar1 = new C1StatusBar();
      this.dockPanel1 = new DockPanel();
      this.rtSupport = new RibbonTab();
      this.ribbonGroup6 = new RibbonGroup();
      this.manualButton = new RibbonButton();
      this.rmUserGuides = new RibbonMenu();
      this.rbUserGuidesInstall = new RibbonButton();
      this.rbUserGuidesExampleProjects = new RibbonButton();
      this.rbUserGuidesDataLimitations = new RibbonButton();
      this.rbUserGuidesUnstratified = new RibbonButton();
      this.rbUserGuidesPrestratified = new RibbonButton();
      this.rbUserGuidesPoststratified = new RibbonButton();
      this.rbUserGuidesStratifyInventory = new RibbonButton();
      this.rbUserGuidesUsingForecast = new RibbonButton();
      this.rbUserGuidesCapturingCoordinateData = new RibbonButton();
      this.rbUserGuidesInternational = new RibbonButton();
      this.rbUserGuidesDifferences_v5_v6 = new RibbonButton();
      this.ribbonSeparator3 = new RibbonSeparator();
      this.rbUserGuidesInventoryImporter = new RibbonButton();
      this.ribbonSeparator2 = new RibbonSeparator();
      this.rbUserGuidesFieldManual = new RibbonButton();
      this.rbiTreeMethods = new RibbonButton();
      this.videoLearningButton = new RibbonButton();
      this.forumButton = new RibbonButton();
      this.visitITreeToolsDotOrgButton = new RibbonButton();
      this.checkForUpdatesButton = new RibbonButton();
      this.aboutButton = new RibbonButton();
      this.rbEula = new RibbonButton();
      this.feedbackButton = new RibbonButton();
      this.ribbonGroup12 = new RibbonGroup();
      this.rbtnShowWhatsNew = new RibbonButton();
      this.rbtnShowChangeLog = new RibbonButton();
      this.rtReports = new RibbonTab();
      this.rgRunModels = new RibbonGroup();
      this.projectMetadataButton = new RibbonButton();
      this.runModelsButton = new RibbonButton();
      this.SendDataButton = new RibbonButton();
      this.RetrieveResultsButton = new RibbonButton();
      this.rgReports = new RibbonGroup();
      this.writtenReportButton = new RibbonButton();
      this.rmCompositionAndStructure = new RibbonMenu();
      this.lblStructureSummary = new RibbonLabel();
      this.rbStructureSummaryBySpecies = new RibbonButton();
      this.rbStructureSummaryByStrataSpecies = new RibbonButton();
      this.NumberOfTreesLabel = new RibbonLabel();
      this.numberOfTreesBySpecies = new RibbonButton();
      this.numberOfTreesByStrataChartMenuItem = new RibbonButton();
      this.numberOfTreesPerUnitAreaByStrataChartMenuItem = new RibbonButton();
      this.rbPublicPrivateByStrata = new RibbonButton();
      this.rbStreetTreesByStrata = new RibbonButton();
      this.SpeciesCompositionLabel = new RibbonLabel();
      this.speciesDistributionByDBHClassChart = new RibbonButton();
      this.speciesCompositionByDBHClassVerticalMenuItem = new RibbonButton();
      this.speciesCompositionByDBHClassHorizontalMenuItem = new RibbonButton();
      this.speciesCompositionByDBHClassAndStrataVerticalMenuItem = new RibbonButton();
      this.speciesCompositionByDBHClassAndStrataHorizontalMenuItem = new RibbonButton();
      this.MostImportantTreeSpeciesLabel = new RibbonLabel();
      this.mostImportantTreeSpeciesMenuItem = new RibbonButton();
      this.SpeciesRichness = new RibbonLabel();
      this.speciesRichnessShannonWienerDiversityIndexMenuItem = new RibbonButton();
      this.OriginOfTreesByStrata = new RibbonLabel();
      this.originOfTreesByStrataMenuItem = new RibbonButton();
      this.ConditionOfTreesLabel = new RibbonLabel();
      this.conditionOfTreesBySpeciesMenuItem = new RibbonButton();
      this.conditionOfTreesByStrataMenuItem = new RibbonButton();
      this.CrownHealthLabel = new RibbonLabel();
      this.conditionOfTreesBySpeciesCustomMenuItem = new RibbonButton();
      this.conditionOfTreesByStratumSpeciesCustomMenuItem = new RibbonButton();
      this.LeafAreaOfTrees = new RibbonLabel();
      this.leafAreaOfTreesByStrataChartMenuItem = new RibbonButton();
      this.leafAreaOfTreesPerUnitAreaByStrataChartMenuItem = new RibbonButton();
      this.LeafAreaAndBiomassLabel = new RibbonLabel();
      this.leafAreaAndBiomassOfShrubsByStrataMenuItem = new RibbonButton();
      this.leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem = new RibbonButton();
      this.GroundCoverCompositionLabel = new RibbonLabel();
      this.groundCoverCompositionByStrataMenuItem = new RibbonButton();
      this.LandUseCompositionLabel = new RibbonLabel();
      this.landUseCompositionByStrataMenuItem = new RibbonButton();
      this.RelativePerformanceIndexLabel = new RibbonLabel();
      this.rbRelativePerformanceIndexBySpecies = new RibbonButton();
      this.MaintenanceLabel = new RibbonLabel();
      this.rbMaintRec = new RibbonButton();
      this.rbMaintRecBySpecies = new RibbonButton();
      this.rbMaintRecReport = new RibbonButton();
      this.rbMaintTask = new RibbonButton();
      this.rbMaintTaskBySpecies = new RibbonButton();
      this.rbMaintTaskReport = new RibbonButton();
      this.rbSidewalkConflicts = new RibbonButton();
      this.rbSidewalkConflictsBySpecies = new RibbonButton();
      this.rbSidewalkConflictsReport = new RibbonButton();
      this.rbMaintUtilityConflicts = new RibbonButton();
      this.rbUtilityConflictsBySpecies = new RibbonButton();
      this.rbUtilityConflictsReport = new RibbonButton();
      this.OtherLabel = new RibbonLabel();
      this.rbFieldOne = new RibbonButton();
      this.rbFieldOneBySpecies = new RibbonButton();
      this.rbFieldOneReport = new RibbonButton();
      this.rbFieldTwo = new RibbonButton();
      this.rbFieldTwoBySpecies = new RibbonButton();
      this.rbFieldTwoReport = new RibbonButton();
      this.rbFieldThree = new RibbonButton();
      this.rbFieldThreeBySpecies = new RibbonButton();
      this.rbFieldThreeReport = new RibbonButton();
      this.rmBenefitsAndCosts = new RibbonMenu();
      this.BenefitsSummaryLabel = new RibbonLabel();
      this.rbBenefitsSummarySpecies = new RibbonButton();
      this.rbBenefitsSummaryStrataSpecies = new RibbonButton();
      this.CarbonStorage = new RibbonLabel();
      this.carbonStorageOfTreesBySpeciesChartMenuItem = new RibbonButton();
      this.carbonStorageOfTreesByStrataChartMenuItem = new RibbonButton();
      this.carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem = new RibbonButton();
      this.AnnualCarbonSequestrationOfTreesLabel = new RibbonLabel();
      this.annualCarbonSequestrationOfTreesBySpeciesChartMenuItem = new RibbonButton();
      this.annualCarbonSequestrationOfTreesByStrataChartMenuItem = new RibbonButton();
      this.annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem = new RibbonButton();
      this.AnnualNetCarbonSequestrationOfTreesLabel = new RibbonLabel();
      this.AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem = new RibbonButton();
      this.AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem = new RibbonButton();
      this.AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem = new RibbonButton();
      this.EnergyEffectsLabel = new RibbonLabel();
      this.energyEffectsOfTreesMenuItem = new RibbonButton();
      this.HydrologyEffectsOfTrees = new RibbonLabel();
      this.HydrologyEffectsOfTreesBySpecies = new RibbonButton();
      this.HydrologyEffectsOfTreesByStratum = new RibbonButton();
      this.OxygenProductionOfTreesLabel = new RibbonLabel();
      this.oxygenProductionOfTreesByStrataChartMenuItem = new RibbonButton();
      this.oxygenProductionPerUnitAreaByStrataChartMenuItem = new RibbonButton();
      this.PollutionRemovalByTreesandShrubs = new RibbonLabel();
      this.monthlyPollutantRemovalByTreesAndShrubsMenuItem = new RibbonButton();
      this.monthlyPollutantRemovalByTreesAndShrubsChartMenuItem = new RibbonButton();
      this.PollutionRemovalByGrass = new RibbonLabel();
      this.monthlyPollutantRemovalByGrassMenuItem = new RibbonButton();
      this.monthlyPollutantRemovalByGrassChartMenuItem = new RibbonButton();
      this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem = new RibbonButton();
      this.BioemissionsOfTreesLabel = new RibbonLabel();
      this.bioemissionsOfTreesBySpeciesMenuItem = new RibbonButton();
      this.bioemissionsOfTreesByStrataMenuItem = new RibbonButton();
      this.UVEffectsOfTreesLabel = new RibbonLabel();
      this.UVEffectsOfTreesByStrata = new RibbonButton();
      this.AllergyIndexOfTreesLabel = new RibbonLabel();
      this.AllergyIndexOfTreesByStratum = new RibbonButton();
      this.WildLifeSuitabilityLabel = new RibbonLabel();
      this.WildLifeSuitabilityByPlots = new RibbonButton();
      this.WildLifeSuitabilityByStrata = new RibbonButton();
      this.ReplacementValueLabel = new RibbonLabel();
      this.ReplacementValueByDBHClassAndSpecies = new RibbonButton();
      this.ManagementCostsLabel = new RibbonLabel();
      this.rbManagementCostsByExpenditure = new RibbonButton();
      this.NetAnnualBenefitsLabel = new RibbonLabel();
      this.rbNetAnnualBenefitsForAllTreesSample = new RibbonButton();
      this.FoodscapeBenefits = new RibbonLabel();
      this.FoodscapeBenefitsofTreesMenuItem = new RibbonButton();
      this.LeafNutrients = new RibbonLabel();
      this.LeafNutrientsofTreesMenuItem = new RibbonButton();
      this.rmIndividualLevelResults = new RibbonMenu();
      this.rlMDDCompositionAndStructure = new RibbonLabel();
      this.rbMDDCompositionOfPlots = new RibbonButton();
      this.rbMDDCompositionOfTrees = new RibbonButton();
      this.rbMDDCompositionOfTreeBySpecies = new RibbonButton();
      this.rbMDDCompositionOfTreesByStratum = new RibbonButton();
      this.rlMDDTreeBenefitsAndCosts = new RibbonLabel();
      this.rbMDDTreeBenefitsSummary = new RibbonButton();
      this.rbMDDTreeBenefitsCarbonStorage = new RibbonButton();
      this.rbMDDTreeBenefitsCarbonSequestration = new RibbonButton();
      this.rbMDDTreeBenefitsEnergyEffects = new RibbonButton();
      this.rbMDDTreeEnergyAvoidedPollutants = new RibbonButton();
      this.rbMDDTreeBenefitsHydrologyEffects = new RibbonButton();
      this.rbMDDTreeBenefitsPollutionRemoval = new RibbonButton();
      this.rbMDDTreeBenefitsOxygenProduction = new RibbonButton();
      this.rbMDDTreeBenefitsVOCEmissions = new RibbonButton();
      this.rbMDDWoodProducts = new RibbonButton();
      this.rlMDDMiscellaneous = new RibbonLabel();
      this.rbMDDMiscPlotComments = new RibbonButton();
      this.rbMDDMiscTreeComments = new RibbonButton();
      this.rbMDDMiscShrubComments = new RibbonButton();
      this.rmAirQualityHealthImpactsAndValuesReports = new RibbonMenu();
      this.airQualityHealthImpactsandValuesbyTrees = new RibbonButton();
      this.airQualityHealthImpactsandValuesbyShrubs = new RibbonButton();
      this.airQualityHealthImpactsandValuesbyGrass = new RibbonButton();
      this.airQualityHealthImpactsandValuesCombined = new RibbonButton();
      this.airQualityHealthImpactsandValuesbyTreesandShrubs = new RibbonButton();
      this.rmPestAnalysis = new RibbonMenu();
      this.SusceptibilityOfTreesToPestsLabel = new RibbonLabel();
      this.SusceptibilityOfTreesToPestsByStrata = new RibbonButton();
      this.PrimaryPestLabel = new RibbonLabel();
      this.primaryPestSummaryOfTreesByStrataButton = new RibbonButton();
      this.primaryPestDetailsOfTreesByStrataButton = new RibbonButton();
      this.SignAndSymptomLabel = new RibbonLabel();
      this.signSymptomOverviewBySpeciesButton = new RibbonButton();
      this.signSymptomDetailsSummaryBySpeciesButton = new RibbonButton();
      this.signSymptomDetailsCompleteBySpeciesButton = new RibbonButton();
      this.signSymptomOverviewByStrataButton = new RibbonButton();
      this.signSymptomDetailsSummaryByStrataButton = new RibbonButton();
      this.signSymptomDetailsCompleteByStrataButton = new RibbonButton();
      this.signSymptomReviewOfTreesButton = new RibbonButton();
      this.PestReviewLabel = new RibbonLabel();
      this.pestReviewOfTreesButton = new RibbonButton();
      this.rgCharts = new RibbonGroup();
      this.rmWeatherReports = new RibbonMenu();
      this.rRawDataLabel = new RibbonLabel();
      this.rbWeatherAirPollutantConcentrationUGM3 = new RibbonButton();
      this.rbWeatherPhotosyntheticallyActiveRadiation = new RibbonButton();
      this.rbWeatherRain = new RibbonButton();
      this.rbWeatherTempF = new RibbonButton();
      this.rbUVIndex = new RibbonButton();
      this.AirQualityImprovementLabel = new RibbonLabel();
      this.rbWeatherAirQualImprovementTree = new RibbonButton();
      this.rbWeatherAirQualImprovementShrub = new RibbonButton();
      this.rbWeatherAirQualImprovementGrass = new RibbonButton();
      this.AirPollutantFluxDryDepositionLabel = new RibbonLabel();
      this.rbWeatherDryDepTree = new RibbonButton();
      this.rbWeatherDryDepShrub = new RibbonButton();
      this.rbWeatherDryDepGrass = new RibbonButton();
      this.TranspirationLabel = new RibbonLabel();
      this.rbTranspirationByTree = new RibbonButton();
      this.rbTranspirationByShrub = new RibbonButton();
      this.rlEvaporation = new RibbonLabel();
      this.rbEvaporationByTrees = new RibbonButton();
      this.rbEvaporationByShrubs = new RibbonButton();
      this.rlWaterInterception = new RibbonLabel();
      this.rbWaterInterceptionByTrees = new RibbonButton();
      this.rbWaterInterceptionByShrubs = new RibbonButton();
      this.rlAvoidedRunoff = new RibbonLabel();
      this.rbAvoidedRunoffByTrees = new RibbonButton();
      this.rbAvoidedRunoffByShrubs = new RibbonButton();
      this.rbPotentialEvapotranspiration = new RibbonLabel();
      this.rbPotentialEvapotranspirationByTrees = new RibbonButton();
      this.rbPotentialEvapotranspirationByShrubs = new RibbonButton();
      this.rbUVIndexReductionLabel = new RibbonLabel();
      this.rbUVIndexReductionByTreesForUnderTrees = new RibbonButton();
      this.rbUVIndexReductionByTreesForOverall = new RibbonButton();
      this.rbIsoprene = new RibbonLabel();
      this.rbIsopreneByTrees = new RibbonButton();
      this.rbIsopreneByShrubs = new RibbonButton();
      this.rbMonoterpene = new RibbonLabel();
      this.rbMonoterpeneByTrees = new RibbonButton();
      this.rbMonoterpeneByShrubs = new RibbonButton();
      this.rgReportTree = new RibbonGroup();
      this.ReportTreeViewToggle = new RibbonToggleButton();
      this.rgSettings = new RibbonGroup();
      this.rbReportsEnglish = new RibbonToggleButton();
      this.rbReportsMetric = new RibbonToggleButton();
      this.ribbonSeparator4 = new RibbonSeparator();
      this.rbReportsSpeciesCN = new RibbonToggleButton();
      this.rbReportsSpeciesSN = new RibbonToggleButton();
      this.ribbonSeparator5 = new RibbonSeparator();
      this.rbCheckBoxGPS = new RibbonCheckBox();
      this.rbCheckBoxComments = new RibbonCheckBox();
      this.rbCheckBoxUID = new RibbonCheckBox();
      this.rbCheckBoxHideZeros = new RibbonCheckBox();
      this.rgModelNotes = new RibbonGroup();
      this.rmbModelProcessingNotes = new RibbonButton();
      this.rgMapping = new RibbonGroup();
      this.rbMapData = new RibbonButton();
      this.rbReportsExportCSV = new RibbonButton();
      this.rbReportsExportKML = new RibbonButton();
      this.rgExport = new RibbonGroup();
      this.rbReportExport = new RibbonButton();
      this.rtForecast = new RibbonTab();
      this.ForecastGroup = new RibbonGroup();
      this.ParametersReportButton = new RibbonButton();
      this.RunForecastButton = new RibbonButton();
      this.BasicOptionsGroup = new RibbonGroup();
      this.ribbonToolBar1 = new RibbonToolBar();
      this.ribbonToolBar2 = new RibbonToolBar();
      this.BasicInputsButton = new RibbonButton();
      this.CustomOptionsGroup = new RibbonGroup();
      this.MortalityButton = new RibbonButton();
      this.ReplantingButton = new RibbonButton();
      this.ExtremeEventsSplitButton = new RibbonMenu();
      this.PestsItem = new RibbonButton();
      this.StormItem = new RibbonButton();
      this.ConfigurationsGroup = new RibbonGroup();
      this.ribbonLabel1 = new RibbonLabel();
      this.ConfigurationComboBox = new RibbonComboBox();
      this.ForecastRenameButton = new RibbonButton();
      this.NewForecastButton = new RibbonButton();
      this.CopyForecastButton = new RibbonButton();
      this.DeleteForecastButton = new RibbonButton();
      this.DefaultsButton = new RibbonButton();
      this.rgForecastReports = new RibbonGroup();
      this.StructuralSplitButton = new RibbonMenu();
      this.ForecastPopulationSummaryLabel = new RibbonLabel();
      this.NumTreesTotal = new RibbonButton();
      this.NumTreesByStrata = new RibbonButton();
      this.PctCoverTotal = new RibbonButton();
      this.PctCoverByStrata = new RibbonButton();
      this.TreeCoverAreaTotal = new RibbonButton();
      this.TreeCoverAreaByStrata = new RibbonButton();
      this.DbhGrowthTotal = new RibbonButton();
      this.DbhGrowthByStrata = new RibbonButton();
      this.DbhDistribTotal = new RibbonButton();
      this.BasalAreaTotal = new RibbonButton();
      this.BasalAreaByStrata = new RibbonButton();
      this.ForecastLeafAreaAndBiomassLabel = new RibbonLabel();
      this.LeafAreaTotal = new RibbonButton();
      this.LeafAreaByStrata = new RibbonButton();
      this.LeafAreaIndexTotal = new RibbonButton();
      this.LeafAreaIndexByStrata = new RibbonButton();
      this.TotalLfBiomassTotal = new RibbonButton();
      this.TotalLfBiomassByStrata = new RibbonButton();
      this.AreaLfBiomassTotal = new RibbonButton();
      this.AreaLfBiomassByStrata = new RibbonButton();
      this.TreeBiomassTotal = new RibbonButton();
      this.TreeBiomassByStrata = new RibbonButton();
      this.FunctionalSplitButton = new RibbonMenu();
      this.ForecastCarbonLabel = new RibbonLabel();
      this.ForecastCarbonStorage = new RibbonButton();
      this.ForecastCarbonStorageByStrata = new RibbonButton();
      this.ForecastCarbonSequestration = new RibbonButton();
      this.ForecastCarbonSequestrationByStrata = new RibbonButton();
      this.ForecastPollutantLabel = new RibbonLabel();
      this.ForecastPollutantSummary = new RibbonButton();
      this.ForecastPollutantCO = new RibbonButton();
      this.ForecastPollutantNO2 = new RibbonButton();
      this.ForecastPollutantO3 = new RibbonButton();
      this.ForecastPollutantSO2 = new RibbonButton();
      this.ForecastPollutantPM25 = new RibbonButton();
      this.ForecastPollutantPM10 = new RibbonButton();
      this.rgForecastUnits = new RibbonGroup();
      this.rtbForecastEnglish = new RibbonToggleButton();
      this.rtbForecastMetric = new RibbonToggleButton();
      this.ForecastLockGroup = new RibbonGroup();
      this.ForecastEditDataButton = new RibbonButton();
      this.rtData = new RibbonTab();
      this.rgDataCollection = new RibbonGroup();
      this.rbSubmitMobileData = new RibbonButton();
      this.rbRetrieveMobileData = new RibbonButton();
      this.rbtnPaperForm1 = new RibbonButton();
      this.rsImport = new RibbonSeparator();
      this.rbImport = new RibbonButton();
      this.rgInventoryData = new RibbonGroup();
      this.rbDataPlots = new RibbonButton();
      this.rbDataReferenceObjects = new RibbonButton();
      this.rbDataGroundCovers = new RibbonButton();
      this.rbDataLandUses = new RibbonButton();
      this.rbDataTrees = new RibbonButton();
      this.rbDataShrubs = new RibbonButton();
      this.rbDataPlantingSites = new RibbonButton();
      this.rbValidate = new RibbonButton();
      this.rgInventoryValue = new RibbonGroup();
      this.rbtnBenefitPrices = new RibbonButton();
      this.rbAnnualCosts = new RibbonButton();
      this.rgReportClasses = new RibbonGroup();
      this.rbRptDBH = new RibbonButton();
      this.rbHealthClasses = new RibbonButton();
      this.rgDataExport = new RibbonGroup();
      this.rbDataExportCSV = new RibbonButton();
      this.rbDataExportKML = new RibbonButton();
      this.rgDataActions = new RibbonGroup();
      this.rbDataNew = new RibbonButton();
      this.rbDataCopy = new RibbonButton();
      this.rbDataUndo = new RibbonButton();
      this.rbDataRedo = new RibbonButton();
      this.rbDataDefaults = new RibbonButton();
      this.rbDataDelete = new RibbonButton();
      this.rgDataMode = new RibbonGroup();
      this.rbDataEnableEdit = new RibbonButton();
      this.rgPlot = new RibbonGroup();
      this.rbtnPlotRefObjects = new RibbonButton();
      this.rbtnPlotLandUses = new RibbonButton();
      this.rbtnPlotGroundCovers = new RibbonButton();
      this.rbtnPlotTrees = new RibbonButton();
      this.rbtnPlotShrubs = new RibbonButton();
      this.rbtnPlotPlantingSites = new RibbonButton();
      this.rtProject = new RibbonTab();
      this.rgProject = new RibbonGroup();
      this.rbtnOverview = new RibbonButton();
      this.rgDataFields = new RibbonGroup();
      this.rbtnLandUses = new RibbonButton();
      this.rbtnGroundCovers = new RibbonButton();
      this.rbDBH = new RibbonButton();
      this.rbCondition = new RibbonButton();
      this.rbDieback = new RibbonButton();
      this.rbtnStreets = new RibbonButton();
      this.rbtnLocSites = new RibbonButton();
      this.rbPlantingSiteTypes = new RibbonButton();
      this.rmMaintenance = new RibbonMenu();
      this.rbtnMaintenanceRecommended = new RibbonButton();
      this.rbtnMaintenanceTasks = new RibbonButton();
      this.rbtnSidewalkConflicts = new RibbonButton();
      this.rbtnUtilityConflicts = new RibbonButton();
      this.rmOther = new RibbonMenu();
      this.rbtnFieldOne = new RibbonButton();
      this.rbtnFieldTwo = new RibbonButton();
      this.rbtnFieldThree = new RibbonButton();
      this.rgDefinePlots = new RibbonGroup();
      this.loadPlotsFromFileButton = new RibbonButton();
      this.definePlotsViaGoogleMaps = new RibbonButton();
      this.definePlotsManuallyButton = new RibbonButton();
      this.rgStrata = new RibbonGroup();
      this.rbStrata = new RibbonButton();
      this.rgProjectExport = new RibbonGroup();
      this.rbProjectExportCSV = new RibbonButton();
      this.rgProjectActions = new RibbonGroup();
      this.rbProjectNew = new RibbonButton();
      this.rbProjectCopy = new RibbonButton();
      this.rbProjectUndo = new RibbonButton();
      this.rbProjectRedo = new RibbonButton();
      this.rbProjectDefaults = new RibbonButton();
      this.rbProjectDelete = new RibbonButton();
      this.rgProjectEditData = new RibbonGroup();
      this.rbProjectEnableEdit = new RibbonButton();
      this.configToolBar = new RibbonConfigToolBar();
      this.minimizeRibbonButton = new RibbonButton();
      this.expandRibbonButton = new RibbonButton();
      this.helpConfigButton = new RibbonButton();
      this.colorSchemeMenu = new RibbonMenu();
      this.themeColorMenu = new RibbonMenu();
      this.azureButton = new RibbonToggleButton();
      this.blueButton = new RibbonToggleButton();
      this.greenButton = new RibbonToggleButton();
      this.orangeButton = new RibbonToggleButton();
      this.orchidButton = new RibbonToggleButton();
      this.redButton = new RibbonToggleButton();
      this.tealButton = new RibbonToggleButton();
      this.violetButton = new RibbonToggleButton();
      this.themeLightnessMenu = new RibbonMenu();
      this.darkGrayButton = new RibbonToggleButton();
      this.lightGrayButton = new RibbonToggleButton();
      this.whiteButton = new RibbonToggleButton();
      this.ribbonSeparator7 = new RibbonSeparator();
      this.customButton = new RibbonToggleButton();
      this.blue2007Button = new RibbonToggleButton();
      this.silver2007Button = new RibbonToggleButton();
      this.black2007Button = new RibbonToggleButton();
      this.blue2010Button = new RibbonToggleButton();
      this.silver2010Button = new RibbonToggleButton();
      this.black2010Button = new RibbonToggleButton();
      this.windows7Button = new RibbonToggleButton();
      this.appMenu = new RibbonApplicationMenu();
      this.exitButton = new RibbonButton();
      this.mnuFileNewProject = new RibbonButton();
      this.mnuFileOpenProject = new RibbonButton();
      this.mnuFileOpenSampleProject = new RibbonButton();
      this.mnuFileCreateReinventory = new RibbonButton();
      this.mnuFileSaveProject = new RibbonButton();
      this.mnuFileSaveProjectAs = new RibbonButton();
      this.mnuFilePackProject = new RibbonButton();
      this.mnuFilePrint = new RibbonButton();
      this.mnuFileExport = new RibbonButton();
      this.mnuFileImport = new RibbonButton();
      this.mnuFileCloseProject = new RibbonButton();
      this.rmRecentProjects = new RibbonListItem();
      this.rlProjectsLabel = new RibbonLabel();
      this.rliSeparartor = new RibbonListItem();
      this.rsRecentProjects = new RibbonSeparator();
      this.undoSplitButton = new RibbonSplitButton();
      this.qat = new RibbonQat();
      this.c1Ribbon1 = new C1.Win.C1Ribbon.C1Ribbon();
      this.rtView = new RibbonTab();
      this.dataEntryGroup = new RibbonGroup();
      this.rcboProject = new RibbonComboBox();
      this.rcboSeries = new RibbonComboBox();
      this.rcboYear = new RibbonComboBox();
      this.rgSpeciesName = new RibbonGroup();
      this.rbSpeciesCN = new RibbonToggleButton();
      this.rbSpeciesSN = new RibbonToggleButton();
      this.rgUnits = new RibbonGroup();
      this.rbEnglish = new RibbonToggleButton();
      this.rbMetric = new RibbonToggleButton();
      this.rgSpecies = new RibbonGroup();
      this.rbtnSpeciesCode = new RibbonButton();
      this.rbSpeciesList = new RibbonButton();
      this.rgViewExport = new RibbonGroup();
      this.rbViewExportCSV = new RibbonButton();
      this.rgConfiguration = new RibbonGroup();
      this.rcbAutoCheckUpdates = new RibbonCheckBox();
      this.rcbMinimizeHelp = new RibbonCheckBox();
      this.ribbonGroup8 = new RibbonGroup();
      RibbonSeparator ribbonSeparator = new RibbonSeparator();
      ((ISupportInitialize) this.c1StatusBar1).BeginInit();
      ((ISupportInitialize) this.c1Ribbon1).BeginInit();
      this.SuspendLayout();
      ribbonSeparator.Name = "ribbonSeparator1";
      this.statusTrackBar.Maximum = 20;
      this.statusTrackBar.Name = "statusTrackBar";
      this.statusTrackBar.StepFrequency = 1;
      this.statusTrackBar.TickFrequency = 5;
      this.statusTrackBar.Value = 5;
      this.viewsMenu.Items.Add((RibbonItem) this.sbPrintLayoutToggleButton);
      this.viewsMenu.Items.Add((RibbonItem) this.sbFullScreenToggleButton);
      this.viewsMenu.Items.Add((RibbonItem) this.sbWebLayoutToggleButton);
      this.viewsMenu.Name = "viewsMenu";
      this.viewsMenu.SmallImage = (Image) componentResourceManager.GetObject("viewsMenu.SmallImage");
      componentResourceManager.ApplyResources((object) this.viewsMenu, "viewsMenu");
      this.sbPrintLayoutToggleButton.CanDepress = false;
      this.sbPrintLayoutToggleButton.Name = "sbPrintLayoutToggleButton";
      this.sbPrintLayoutToggleButton.Pressed = true;
      this.sbPrintLayoutToggleButton.SmallImage = (Image) componentResourceManager.GetObject("sbPrintLayoutToggleButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.sbPrintLayoutToggleButton, "sbPrintLayoutToggleButton");
      this.sbPrintLayoutToggleButton.ToggleGroupName = "viewsGroup";
      this.sbFullScreenToggleButton.CanDepress = false;
      this.sbFullScreenToggleButton.Name = "sbFullScreenToggleButton";
      this.sbFullScreenToggleButton.SmallImage = (Image) componentResourceManager.GetObject("sbFullScreenToggleButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.sbFullScreenToggleButton, "sbFullScreenToggleButton");
      this.sbFullScreenToggleButton.ToggleGroupName = "viewsGroup";
      this.sbWebLayoutToggleButton.CanDepress = false;
      this.sbWebLayoutToggleButton.Name = "sbWebLayoutToggleButton";
      this.sbWebLayoutToggleButton.SmallImage = (Image) componentResourceManager.GetObject("sbWebLayoutToggleButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.sbWebLayoutToggleButton, "sbWebLayoutToggleButton");
      this.sbWebLayoutToggleButton.ToggleGroupName = "viewsGroup";
      this.sbProectInfoLabel.Name = "sbProectInfoLabel";
      componentResourceManager.ApplyResources((object) this.sbProectInfoLabel, "sbProectInfoLabel");
      this.ribbonSeparator13.Name = "ribbonSeparator13";
      this.c1StatusBar1.LeftPaneItems.Add((RibbonItem) this.ribbonSeparator13);
      this.c1StatusBar1.LeftPaneItems.Add((RibbonItem) this.sbProectInfoLabel);
      this.c1StatusBar1.Location = new Point(0, 1018);
      componentResourceManager.ApplyResources((object) this.c1StatusBar1, "c1StatusBar1");
      this.c1StatusBar1.Name = "c1StatusBar1";
      this.c1StatusBar1.RightPaneItems.Add((RibbonItem) this.viewsMenu);
      this.c1StatusBar1.RightPaneItems.Add((RibbonItem) this.statusTrackBar);
      this.c1StatusBar1.Size = new Size(1791, 34);
      componentResourceManager.ApplyResources((object) this.dockPanel1, "dockPanel1");
      this.dockPanel1.DockBackColor = System.Drawing.Color.FromArgb(207, 221, 238);
      this.dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
      this.dockPanel1.Name = "dockPanel1";
      this.dockPanel1.ActiveDocumentChanged += new EventHandler(this.dockPanel1_ActiveDocumentChanged);
      this.rtSupport.Groups.Add(this.ribbonGroup6);
      this.rtSupport.Groups.Add(this.ribbonGroup12);
      this.rtSupport.Name = "rtSupport";
      componentResourceManager.ApplyResources((object) this.rtSupport, "rtSupport");
      this.rtSupport.DoubleClick += new EventHandler(this.rbCheckBoxComments_CheckedChanged);
      this.ribbonGroup6.Items.Add((RibbonItem) this.manualButton);
      this.ribbonGroup6.Items.Add((RibbonItem) this.rmUserGuides);
      this.ribbonGroup6.Items.Add((RibbonItem) this.rbiTreeMethods);
      this.ribbonGroup6.Items.Add((RibbonItem) this.videoLearningButton);
      this.ribbonGroup6.Items.Add((RibbonItem) this.forumButton);
      this.ribbonGroup6.Items.Add((RibbonItem) this.visitITreeToolsDotOrgButton);
      this.ribbonGroup6.Items.Add((RibbonItem) this.checkForUpdatesButton);
      this.ribbonGroup6.Items.Add((RibbonItem) this.aboutButton);
      this.ribbonGroup6.Items.Add((RibbonItem) this.rbEula);
      this.ribbonGroup6.Items.Add((RibbonItem) this.feedbackButton);
      this.ribbonGroup6.Name = "ribbonGroup6";
      this.manualButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.User_Manual_32;
      this.manualButton.Name = "manualButton";
      this.manualButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.User_Manual_16;
      componentResourceManager.ApplyResources((object) this.manualButton, "manualButton");
      this.manualButton.Click += new EventHandler(this.manualButton_Click);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesInstall);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesExampleProjects);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesDataLimitations);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesUnstratified);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesPrestratified);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesPoststratified);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesStratifyInventory);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesUsingForecast);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesCapturingCoordinateData);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesInternational);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesDifferences_v5_v6);
      this.rmUserGuides.Items.Add((RibbonItem) this.ribbonSeparator3);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesInventoryImporter);
      this.rmUserGuides.Items.Add((RibbonItem) this.ribbonSeparator2);
      this.rmUserGuides.Items.Add((RibbonItem) this.rbUserGuidesFieldManual);
      this.rmUserGuides.LargeImage = (Image) componentResourceManager.GetObject("rmUserGuides.LargeImage");
      this.rmUserGuides.Name = "rmUserGuides";
      componentResourceManager.ApplyResources((object) this.rmUserGuides, "rmUserGuides");
      this.rbUserGuidesInstall.Name = "rbUserGuidesInstall";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesInstall, "rbUserGuidesInstall");
      this.rbUserGuidesInstall.Click += new EventHandler(this.rbUserGuidesInstall_Click);
      this.rbUserGuidesExampleProjects.Name = "rbUserGuidesExampleProjects";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesExampleProjects, "rbUserGuidesExampleProjects");
      this.rbUserGuidesExampleProjects.Click += new EventHandler(this.rbUserGuidesExampleProjects_Click);
      this.rbUserGuidesDataLimitations.Name = "rbUserGuidesDataLimitations";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesDataLimitations, "rbUserGuidesDataLimitations");
      this.rbUserGuidesDataLimitations.Click += new EventHandler(this.rbUserGuidesDataLimitations_Click);
      this.rbUserGuidesUnstratified.Name = "rbUserGuidesUnstratified";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesUnstratified, "rbUserGuidesUnstratified");
      this.rbUserGuidesUnstratified.Click += new EventHandler(this.rbUserGuidesUnstratified_Click);
      this.rbUserGuidesPrestratified.Name = "rbUserGuidesPrestratified";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesPrestratified, "rbUserGuidesPrestratified");
      this.rbUserGuidesPrestratified.Click += new EventHandler(this.rbUserGuidesPrestratified_Click);
      this.rbUserGuidesPoststratified.Name = "rbUserGuidesPoststratified";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesPoststratified, "rbUserGuidesPoststratified");
      this.rbUserGuidesPoststratified.Click += new EventHandler(this.rbUserGuidesPoststratified_Click);
      this.rbUserGuidesStratifyInventory.Name = "rbUserGuidesStratifyInventory";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesStratifyInventory, "rbUserGuidesStratifyInventory");
      this.rbUserGuidesStratifyInventory.Click += new EventHandler(this.rbUserGuidesStratifyInventory_Click);
      this.rbUserGuidesUsingForecast.Name = "rbUserGuidesUsingForecast";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesUsingForecast, "rbUserGuidesUsingForecast");
      this.rbUserGuidesUsingForecast.Click += new EventHandler(this.rbUserGuidesUsingForecast_Click);
      this.rbUserGuidesCapturingCoordinateData.Name = "rbUserGuidesCapturingCoordinateData";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesCapturingCoordinateData, "rbUserGuidesCapturingCoordinateData");
      this.rbUserGuidesCapturingCoordinateData.Click += new EventHandler(this.rbUserGuidesCapturingCoordinateData_Click);
      this.rbUserGuidesInternational.Name = "rbUserGuidesInternational";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesInternational, "rbUserGuidesInternational");
      this.rbUserGuidesInternational.Click += new EventHandler(this.rbUserGuidesInternational_Click);
      this.rbUserGuidesDifferences_v5_v6.Name = "rbUserGuidesDifferences_v5_v6";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesDifferences_v5_v6, "rbUserGuidesDifferences_v5_v6");
      this.rbUserGuidesDifferences_v5_v6.Click += new EventHandler(this.rbUserGuidesDifferences_v5_v6_Click);
      this.ribbonSeparator3.Name = "ribbonSeparator3";
      this.rbUserGuidesInventoryImporter.Name = "rbUserGuidesInventoryImporter";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesInventoryImporter, "rbUserGuidesInventoryImporter");
      this.rbUserGuidesInventoryImporter.Click += new EventHandler(this.rbUserGuidesInventoryImporter_Click);
      this.ribbonSeparator2.Name = "ribbonSeparator2";
      this.rbUserGuidesFieldManual.Name = "rbUserGuidesFieldManual";
      componentResourceManager.ApplyResources((object) this.rbUserGuidesFieldManual, "rbUserGuidesFieldManual");
      this.rbUserGuidesFieldManual.Click += new EventHandler(this.rbUserGuidesFieldManual_Click);
      this.rbiTreeMethods.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.iTreeMethods32;
      this.rbiTreeMethods.Name = "rbiTreeMethods";
      this.rbiTreeMethods.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.iTreeMethods16;
      componentResourceManager.ApplyResources((object) this.rbiTreeMethods, "rbiTreeMethods");
      this.rbiTreeMethods.Click += new EventHandler(this.rbiTreeMethods_Click);
      this.videoLearningButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Video_Learning_32;
      this.videoLearningButton.Name = "videoLearningButton";
      this.videoLearningButton.SmallImage = (Image) componentResourceManager.GetObject("videoLearningButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.videoLearningButton, "videoLearningButton");
      this.videoLearningButton.Click += new EventHandler(this.videoLearningButton_Click);
      this.forumButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.User_Forum_32;
      this.forumButton.Name = "forumButton";
      this.forumButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.User_Forum_16;
      componentResourceManager.ApplyResources((object) this.forumButton, "forumButton");
      this.forumButton.Click += new EventHandler(this.forumButton_Click);
      this.visitITreeToolsDotOrgButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.website_32;
      this.visitITreeToolsDotOrgButton.Name = "visitITreeToolsDotOrgButton";
      this.visitITreeToolsDotOrgButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.website_16;
      componentResourceManager.ApplyResources((object) this.visitITreeToolsDotOrgButton, "visitITreeToolsDotOrgButton");
      this.visitITreeToolsDotOrgButton.Click += new EventHandler(this.visitITreeToolsDotOrgButton_Click);
      this.checkForUpdatesButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Software_Update_32;
      this.checkForUpdatesButton.Name = "checkForUpdatesButton";
      this.checkForUpdatesButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.Software_Update_16;
      componentResourceManager.ApplyResources((object) this.checkForUpdatesButton, "checkForUpdatesButton");
      this.checkForUpdatesButton.Click += new EventHandler(this.checkForUpdatesButton_Click);
      this.aboutButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.about_32;
      this.aboutButton.Name = "aboutButton";
      this.aboutButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.about_16;
      componentResourceManager.ApplyResources((object) this.aboutButton, "aboutButton");
      this.aboutButton.Click += new EventHandler(this.aboutButton_Click);
      this.rbEula.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.shield32;
      this.rbEula.Name = "rbEula";
      componentResourceManager.ApplyResources((object) this.rbEula, "rbEula");
      this.rbEula.Click += new EventHandler(this.rbEula_Click);
      this.feedbackButton.LargeImage = (Image) componentResourceManager.GetObject("feedbackButton.LargeImage");
      this.feedbackButton.Name = "feedbackButton";
      this.feedbackButton.SmallImage = (Image) componentResourceManager.GetObject("feedbackButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.feedbackButton, "feedbackButton");
      this.feedbackButton.Click += new EventHandler(this.feedbackButton_Click);
      this.ribbonGroup12.Items.Add((RibbonItem) this.rbtnShowWhatsNew);
      this.ribbonGroup12.Items.Add((RibbonItem) this.rbtnShowChangeLog);
      this.ribbonGroup12.Name = "ribbonGroup12";
      this.rbtnShowWhatsNew.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.WhatsNew_32x32;
      this.rbtnShowWhatsNew.Name = "rbtnShowWhatsNew";
      this.rbtnShowWhatsNew.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.WhatsNew_16x16;
      componentResourceManager.ApplyResources((object) this.rbtnShowWhatsNew, "rbtnShowWhatsNew");
      this.rbtnShowWhatsNew.Click += new EventHandler(this.rbtnShowWhatsNew_Click);
      this.rbtnShowChangeLog.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.changeLog_Large;
      this.rbtnShowChangeLog.Name = "rbtnShowChangeLog";
      this.rbtnShowChangeLog.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.changeLog_Small;
      componentResourceManager.ApplyResources((object) this.rbtnShowChangeLog, "rbtnShowChangeLog");
      this.rbtnShowChangeLog.Click += new EventHandler(this.rbtnShowChangeLog_Click);
      this.rtReports.Enabled = false;
      this.rtReports.Groups.Add(this.rgRunModels);
      this.rtReports.Groups.Add(this.rgReports);
      this.rtReports.Groups.Add(this.rgCharts);
      this.rtReports.Groups.Add(this.rgReportTree);
      this.rtReports.Groups.Add(this.rgSettings);
      this.rtReports.Groups.Add(this.rgModelNotes);
      this.rtReports.Groups.Add(this.rgMapping);
      this.rtReports.Groups.Add(this.rgExport);
      this.rtReports.Name = "rtReports";
      componentResourceManager.ApplyResources((object) this.rtReports, "rtReports");
      this.rgRunModels.Items.Add((RibbonItem) this.projectMetadataButton);
      this.rgRunModels.Items.Add((RibbonItem) this.runModelsButton);
      this.rgRunModels.Items.Add((RibbonItem) this.SendDataButton);
      this.rgRunModels.Items.Add((RibbonItem) this.RetrieveResultsButton);
      this.rgRunModels.Name = "rgRunModels";
      this.projectMetadataButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.project_metadata_Large;
      this.projectMetadataButton.Name = "projectMetadataButton";
      componentResourceManager.ApplyResources((object) this.projectMetadataButton, "projectMetadataButton");
      this.projectMetadataButton.Click += new EventHandler(this.projectMetadataButton_Click);
      this.runModelsButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Run_Models;
      this.runModelsButton.Name = "runModelsButton";
      componentResourceManager.ApplyResources((object) this.runModelsButton, "runModelsButton");
      this.runModelsButton.Click += new EventHandler(this.runModelsButton_Click);
      this.SendDataButton.LargeImage = (Image) componentResourceManager.GetObject("SendDataButton.LargeImage");
      this.SendDataButton.Name = "SendDataButton";
      componentResourceManager.ApplyResources((object) this.SendDataButton, "SendDataButton");
      this.SendDataButton.Click += new EventHandler(this.SendDataButton_Click);
      this.RetrieveResultsButton.LargeImage = (Image) componentResourceManager.GetObject("RetrieveResultsButton.LargeImage");
      this.RetrieveResultsButton.Name = "RetrieveResultsButton";
      componentResourceManager.ApplyResources((object) this.RetrieveResultsButton, "RetrieveResultsButton");
      this.RetrieveResultsButton.Click += new EventHandler(this.RetrieveResultsButton_Click);
      this.rgReports.Items.Add((RibbonItem) this.writtenReportButton);
      this.rgReports.Items.Add((RibbonItem) this.rmCompositionAndStructure);
      this.rgReports.Items.Add((RibbonItem) this.rmBenefitsAndCosts);
      this.rgReports.Items.Add((RibbonItem) this.rmIndividualLevelResults);
      this.rgReports.Items.Add((RibbonItem) this.rmAirQualityHealthImpactsAndValuesReports);
      this.rgReports.Items.Add((RibbonItem) this.rmPestAnalysis);
      this.rgReports.Name = "rgReports";
      componentResourceManager.ApplyResources((object) this.rgReports, "rgReports");
      this.writtenReportButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.written_report_Large;
      this.writtenReportButton.Name = "writtenReportButton";
      componentResourceManager.ApplyResources((object) this.writtenReportButton, "writtenReportButton");
      this.writtenReportButton.Click += new EventHandler(this.writtenReportButton_Click);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.lblStructureSummary);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbStructureSummaryBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbStructureSummaryByStrataSpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.NumberOfTreesLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.numberOfTreesBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.numberOfTreesByStrataChartMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.numberOfTreesPerUnitAreaByStrataChartMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbPublicPrivateByStrata);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbStreetTreesByStrata);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.SpeciesCompositionLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.speciesDistributionByDBHClassChart);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.speciesCompositionByDBHClassVerticalMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.speciesCompositionByDBHClassHorizontalMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.speciesCompositionByDBHClassAndStrataVerticalMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.speciesCompositionByDBHClassAndStrataHorizontalMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.MostImportantTreeSpeciesLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.mostImportantTreeSpeciesMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.SpeciesRichness);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.speciesRichnessShannonWienerDiversityIndexMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.OriginOfTreesByStrata);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.originOfTreesByStrataMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.ConditionOfTreesLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.conditionOfTreesBySpeciesMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.conditionOfTreesByStrataMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.CrownHealthLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.conditionOfTreesBySpeciesCustomMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.conditionOfTreesByStratumSpeciesCustomMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.LeafAreaOfTrees);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.leafAreaOfTreesByStrataChartMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.leafAreaOfTreesPerUnitAreaByStrataChartMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.LeafAreaAndBiomassLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.leafAreaAndBiomassOfShrubsByStrataMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.GroundCoverCompositionLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.groundCoverCompositionByStrataMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.LandUseCompositionLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.landUseCompositionByStrataMenuItem);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.RelativePerformanceIndexLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbRelativePerformanceIndexBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.MaintenanceLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbMaintRec);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbMaintRecBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbMaintRecReport);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbMaintTask);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbMaintTaskBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbMaintTaskReport);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbSidewalkConflicts);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbSidewalkConflictsBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbSidewalkConflictsReport);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbMaintUtilityConflicts);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbUtilityConflictsBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbUtilityConflictsReport);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.OtherLabel);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldOne);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldOneBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldOneReport);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldTwo);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldTwoBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldTwoReport);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldThree);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldThreeBySpecies);
      this.rmCompositionAndStructure.Items.Add((RibbonItem) this.rbFieldThreeReport);
      this.rmCompositionAndStructure.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.structural_Large;
      this.rmCompositionAndStructure.Name = "rmCompositionAndStructure";
      this.rmCompositionAndStructure.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.rmCompositionAndStructure, "rmCompositionAndStructure");
      this.lblStructureSummary.Name = "lblStructureSummary";
      componentResourceManager.ApplyResources((object) this.lblStructureSummary, "lblStructureSummary");
      this.rbStructureSummaryBySpecies.Name = "rbStructureSummaryBySpecies";
      componentResourceManager.ApplyResources((object) this.rbStructureSummaryBySpecies, "rbStructureSummaryBySpecies");
      this.rbStructureSummaryBySpecies.Click += new EventHandler(this.rbStructureSummaryBySpecies_Click);
      this.rbStructureSummaryByStrataSpecies.Name = "rbStructureSummaryByStrataSpecies";
      componentResourceManager.ApplyResources((object) this.rbStructureSummaryByStrataSpecies, "rbStructureSummaryByStrataSpecies");
      this.rbStructureSummaryByStrataSpecies.Click += new EventHandler(this.rbStructureSummaryByStrataSpecies_Click);
      this.NumberOfTreesLabel.Name = "NumberOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.NumberOfTreesLabel, "NumberOfTreesLabel");
      this.numberOfTreesBySpecies.Name = "numberOfTreesBySpecies";
      componentResourceManager.ApplyResources((object) this.numberOfTreesBySpecies, "numberOfTreesBySpecies");
      this.numberOfTreesBySpecies.Click += new EventHandler(this.numberOfTreesBySpecies_Click);
      this.numberOfTreesByStrataChartMenuItem.Name = "numberOfTreesByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.numberOfTreesByStrataChartMenuItem, "numberOfTreesByStrataChartMenuItem");
      this.numberOfTreesByStrataChartMenuItem.Click += new EventHandler(this.numberOfTreesByStrataChartMenuItem_Click);
      this.numberOfTreesPerUnitAreaByStrataChartMenuItem.Name = "numberOfTreesPerUnitAreaByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.numberOfTreesPerUnitAreaByStrataChartMenuItem, "numberOfTreesPerUnitAreaByStrataChartMenuItem");
      this.numberOfTreesPerUnitAreaByStrataChartMenuItem.Click += new EventHandler(this.numberOfTreesPerUnitAreaByStrataChartMenuItem_Click);
      this.rbPublicPrivateByStrata.Name = "rbPublicPrivateByStrata";
      componentResourceManager.ApplyResources((object) this.rbPublicPrivateByStrata, "rbPublicPrivateByStrata");
      this.rbPublicPrivateByStrata.Click += new EventHandler(this.rbPublicPrivateByStrata_Click);
      this.rbStreetTreesByStrata.Name = "rbStreetTreesByStrata";
      componentResourceManager.ApplyResources((object) this.rbStreetTreesByStrata, "rbStreetTreesByStrata");
      this.rbStreetTreesByStrata.Click += new EventHandler(this.rbStreetTreesByStrata_Click);
      this.SpeciesCompositionLabel.Name = "SpeciesCompositionLabel";
      componentResourceManager.ApplyResources((object) this.SpeciesCompositionLabel, "SpeciesCompositionLabel");
      this.speciesDistributionByDBHClassChart.Name = "speciesDistributionByDBHClassChart";
      componentResourceManager.ApplyResources((object) this.speciesDistributionByDBHClassChart, "speciesDistributionByDBHClassChart");
      this.speciesDistributionByDBHClassChart.Click += new EventHandler(this.speciesDistributionByDBHClassChart_Click);
      this.speciesCompositionByDBHClassVerticalMenuItem.Name = "speciesCompositionByDBHClassVerticalMenuItem";
      componentResourceManager.ApplyResources((object) this.speciesCompositionByDBHClassVerticalMenuItem, "speciesCompositionByDBHClassVerticalMenuItem");
      this.speciesCompositionByDBHClassVerticalMenuItem.Click += new EventHandler(this.speciesCompositionByDBHClassMenuItem_Click);
      this.speciesCompositionByDBHClassHorizontalMenuItem.Name = "speciesCompositionByDBHClassHorizontalMenuItem";
      componentResourceManager.ApplyResources((object) this.speciesCompositionByDBHClassHorizontalMenuItem, "speciesCompositionByDBHClassHorizontalMenuItem");
      this.speciesCompositionByDBHClassHorizontalMenuItem.Click += new EventHandler(this.speciesCompositionByDBHClassHorizontalMenuItem_Click);
      this.speciesCompositionByDBHClassAndStrataVerticalMenuItem.Name = "speciesCompositionByDBHClassAndStrataVerticalMenuItem";
      componentResourceManager.ApplyResources((object) this.speciesCompositionByDBHClassAndStrataVerticalMenuItem, "speciesCompositionByDBHClassAndStrataVerticalMenuItem");
      this.speciesCompositionByDBHClassAndStrataVerticalMenuItem.Click += new EventHandler(this.speciesCompositionByDBHClassAndStrataMenuItem_Click);
      this.speciesCompositionByDBHClassAndStrataHorizontalMenuItem.Name = "speciesCompositionByDBHClassAndStrataHorizontalMenuItem";
      componentResourceManager.ApplyResources((object) this.speciesCompositionByDBHClassAndStrataHorizontalMenuItem, "speciesCompositionByDBHClassAndStrataHorizontalMenuItem");
      this.speciesCompositionByDBHClassAndStrataHorizontalMenuItem.Click += new EventHandler(this.speciesCompositionByDBHClassAndStrataHorizontalMenuItem_Click);
      this.MostImportantTreeSpeciesLabel.Name = "MostImportantTreeSpeciesLabel";
      componentResourceManager.ApplyResources((object) this.MostImportantTreeSpeciesLabel, "MostImportantTreeSpeciesLabel");
      this.mostImportantTreeSpeciesMenuItem.Name = "mostImportantTreeSpeciesMenuItem";
      componentResourceManager.ApplyResources((object) this.mostImportantTreeSpeciesMenuItem, "mostImportantTreeSpeciesMenuItem");
      this.mostImportantTreeSpeciesMenuItem.Click += new EventHandler(this.mostImportantTreeSpeciesMenuItem_Click);
      this.SpeciesRichness.Name = "SpeciesRichness";
      componentResourceManager.ApplyResources((object) this.SpeciesRichness, "SpeciesRichness");
      this.speciesRichnessShannonWienerDiversityIndexMenuItem.Name = "speciesRichnessShannonWienerDiversityIndexMenuItem";
      componentResourceManager.ApplyResources((object) this.speciesRichnessShannonWienerDiversityIndexMenuItem, "speciesRichnessShannonWienerDiversityIndexMenuItem");
      this.speciesRichnessShannonWienerDiversityIndexMenuItem.Click += new EventHandler(this.speciesRichnessShannonWienerDiversityIndexMenuItem_Click);
      this.OriginOfTreesByStrata.Name = "OriginOfTreesByStrata";
      componentResourceManager.ApplyResources((object) this.OriginOfTreesByStrata, "OriginOfTreesByStrata");
      this.originOfTreesByStrataMenuItem.Name = "originOfTreesByStrataMenuItem";
      componentResourceManager.ApplyResources((object) this.originOfTreesByStrataMenuItem, "originOfTreesByStrataMenuItem");
      this.originOfTreesByStrataMenuItem.Click += new EventHandler(this.originOfTreesByStrataMenuItem_Click);
      this.ConditionOfTreesLabel.Name = "ConditionOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.ConditionOfTreesLabel, "ConditionOfTreesLabel");
      this.conditionOfTreesBySpeciesMenuItem.Name = "conditionOfTreesBySpeciesMenuItem";
      componentResourceManager.ApplyResources((object) this.conditionOfTreesBySpeciesMenuItem, "conditionOfTreesBySpeciesMenuItem");
      this.conditionOfTreesBySpeciesMenuItem.Click += new EventHandler(this.conditionOfTreesBySpeciesMenuItem_Click);
      this.conditionOfTreesByStrataMenuItem.Name = "conditionOfTreesByStrataMenuItem";
      componentResourceManager.ApplyResources((object) this.conditionOfTreesByStrataMenuItem, "conditionOfTreesByStrataMenuItem");
      this.conditionOfTreesByStrataMenuItem.Click += new EventHandler(this.conditionOfTreesByStrataMenuItem_Click);
      this.CrownHealthLabel.Name = "CrownHealthLabel";
      componentResourceManager.ApplyResources((object) this.CrownHealthLabel, "CrownHealthLabel");
      this.conditionOfTreesBySpeciesCustomMenuItem.Name = "conditionOfTreesBySpeciesCustomMenuItem";
      componentResourceManager.ApplyResources((object) this.conditionOfTreesBySpeciesCustomMenuItem, "conditionOfTreesBySpeciesCustomMenuItem");
      this.conditionOfTreesBySpeciesCustomMenuItem.Click += new EventHandler(this.conditionOfTreesBySpeciesCustomMenuItem_Click);
      this.conditionOfTreesByStratumSpeciesCustomMenuItem.Name = "conditionOfTreesByStratumSpeciesCustomMenuItem";
      componentResourceManager.ApplyResources((object) this.conditionOfTreesByStratumSpeciesCustomMenuItem, "conditionOfTreesByStratumSpeciesCustomMenuItem");
      this.conditionOfTreesByStratumSpeciesCustomMenuItem.Click += new EventHandler(this.conditionOfTreesByStratumSpeciesCustomMenuItem_Click);
      this.LeafAreaOfTrees.Name = "LeafAreaOfTrees";
      componentResourceManager.ApplyResources((object) this.LeafAreaOfTrees, "LeafAreaOfTrees");
      this.leafAreaOfTreesByStrataChartMenuItem.Name = "leafAreaOfTreesByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.leafAreaOfTreesByStrataChartMenuItem, "leafAreaOfTreesByStrataChartMenuItem");
      this.leafAreaOfTreesByStrataChartMenuItem.Click += new EventHandler(this.leafAreaOfTreesByStrataChartMenuItem_Click);
      this.leafAreaOfTreesPerUnitAreaByStrataChartMenuItem.Name = "leafAreaOfTreesPerUnitAreaByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.leafAreaOfTreesPerUnitAreaByStrataChartMenuItem, "leafAreaOfTreesPerUnitAreaByStrataChartMenuItem");
      this.leafAreaOfTreesPerUnitAreaByStrataChartMenuItem.Click += new EventHandler(this.leafAreaOfTreesPerUnitAreaByStrataChartMenuItem_Click);
      this.LeafAreaAndBiomassLabel.Name = "LeafAreaAndBiomassLabel";
      componentResourceManager.ApplyResources((object) this.LeafAreaAndBiomassLabel, "LeafAreaAndBiomassLabel");
      this.leafAreaAndBiomassOfShrubsByStrataMenuItem.Name = "leafAreaAndBiomassOfShrubsByStrataMenuItem";
      componentResourceManager.ApplyResources((object) this.leafAreaAndBiomassOfShrubsByStrataMenuItem, "leafAreaAndBiomassOfShrubsByStrataMenuItem");
      this.leafAreaAndBiomassOfShrubsByStrataMenuItem.Click += new EventHandler(this.leafAreaAndBiomassOfShrubsByStrataMenuItem_Click);
      this.leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem.Name = "leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem";
      componentResourceManager.ApplyResources((object) this.leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem, "leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem");
      this.leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem.Click += new EventHandler(this.leafAreaAndBiomassOfTreesAndShrubsByStrataMenuItem_Click);
      this.GroundCoverCompositionLabel.Name = "GroundCoverCompositionLabel";
      componentResourceManager.ApplyResources((object) this.GroundCoverCompositionLabel, "GroundCoverCompositionLabel");
      this.groundCoverCompositionByStrataMenuItem.Name = "groundCoverCompositionByStrataMenuItem";
      componentResourceManager.ApplyResources((object) this.groundCoverCompositionByStrataMenuItem, "groundCoverCompositionByStrataMenuItem");
      this.groundCoverCompositionByStrataMenuItem.Click += new EventHandler(this.groundCoverCompositionByStrataMenuItem_Click);
      this.LandUseCompositionLabel.Name = "LandUseCompositionLabel";
      componentResourceManager.ApplyResources((object) this.LandUseCompositionLabel, "LandUseCompositionLabel");
      this.landUseCompositionByStrataMenuItem.Name = "landUseCompositionByStrataMenuItem";
      componentResourceManager.ApplyResources((object) this.landUseCompositionByStrataMenuItem, "landUseCompositionByStrataMenuItem");
      this.landUseCompositionByStrataMenuItem.Click += new EventHandler(this.landUseCompositionByStrataMenuItem_Click);
      this.RelativePerformanceIndexLabel.Name = "RelativePerformanceIndexLabel";
      componentResourceManager.ApplyResources((object) this.RelativePerformanceIndexLabel, "RelativePerformanceIndexLabel");
      this.rbRelativePerformanceIndexBySpecies.Name = "rbRelativePerformanceIndexBySpecies";
      componentResourceManager.ApplyResources((object) this.rbRelativePerformanceIndexBySpecies, "rbRelativePerformanceIndexBySpecies");
      this.rbRelativePerformanceIndexBySpecies.Click += new EventHandler(this.rbRelativePerformanceIndexBySpecies_Click);
      this.MaintenanceLabel.Name = "MaintenanceLabel";
      componentResourceManager.ApplyResources((object) this.MaintenanceLabel, "MaintenanceLabel");
      this.rbMaintRec.Name = "rbMaintRec";
      componentResourceManager.ApplyResources((object) this.rbMaintRec, "rbMaintRec");
      this.rbMaintRec.Click += new EventHandler(this.rbMaintRec_Click);
      this.rbMaintRecBySpecies.Name = "rbMaintRecBySpecies";
      componentResourceManager.ApplyResources((object) this.rbMaintRecBySpecies, "rbMaintRecBySpecies");
      this.rbMaintRecBySpecies.Click += new EventHandler(this.rbMaintRecBySpecies_Click);
      this.rbMaintRecReport.Name = "rbMaintRecReport";
      componentResourceManager.ApplyResources((object) this.rbMaintRecReport, "rbMaintRecReport");
      this.rbMaintRecReport.Click += new EventHandler(this.rbMaintRecReport_Click);
      this.rbMaintTask.Name = "rbMaintTask";
      componentResourceManager.ApplyResources((object) this.rbMaintTask, "rbMaintTask");
      this.rbMaintTask.Click += new EventHandler(this.rbMaintTask_Click);
      this.rbMaintTaskBySpecies.Name = "rbMaintTaskBySpecies";
      componentResourceManager.ApplyResources((object) this.rbMaintTaskBySpecies, "rbMaintTaskBySpecies");
      this.rbMaintTaskBySpecies.Click += new EventHandler(this.rbMaintTaskBySpecies_Click);
      this.rbMaintTaskReport.Name = "rbMaintTaskReport";
      componentResourceManager.ApplyResources((object) this.rbMaintTaskReport, "rbMaintTaskReport");
      this.rbMaintTaskReport.Click += new EventHandler(this.rbMaintTaskReport_Click);
      this.rbSidewalkConflicts.Name = "rbSidewalkConflicts";
      componentResourceManager.ApplyResources((object) this.rbSidewalkConflicts, "rbSidewalkConflicts");
      this.rbSidewalkConflicts.Click += new EventHandler(this.rbSidewalkConflicts_Click);
      this.rbSidewalkConflictsBySpecies.Name = "rbSidewalkConflictsBySpecies";
      componentResourceManager.ApplyResources((object) this.rbSidewalkConflictsBySpecies, "rbSidewalkConflictsBySpecies");
      this.rbSidewalkConflictsBySpecies.Click += new EventHandler(this.rbSidewalkConflictsBySpecies_Click);
      this.rbSidewalkConflictsReport.Name = "rbSidewalkConflictsReport";
      componentResourceManager.ApplyResources((object) this.rbSidewalkConflictsReport, "rbSidewalkConflictsReport");
      this.rbSidewalkConflictsReport.Click += new EventHandler(this.rbSidewalkConflictsReport_Click);
      this.rbMaintUtilityConflicts.Name = "rbMaintUtilityConflicts";
      componentResourceManager.ApplyResources((object) this.rbMaintUtilityConflicts, "rbMaintUtilityConflicts");
      this.rbMaintUtilityConflicts.Click += new EventHandler(this.rbMaintUtilityConflicts_Click);
      this.rbUtilityConflictsBySpecies.Name = "rbUtilityConflictsBySpecies";
      componentResourceManager.ApplyResources((object) this.rbUtilityConflictsBySpecies, "rbUtilityConflictsBySpecies");
      this.rbUtilityConflictsBySpecies.Click += new EventHandler(this.rbUtilityConflictsBySpecies_Click);
      this.rbUtilityConflictsReport.Name = "rbUtilityConflictsReport";
      componentResourceManager.ApplyResources((object) this.rbUtilityConflictsReport, "rbUtilityConflictsReport");
      this.rbUtilityConflictsReport.Click += new EventHandler(this.rbUtilityConflictsReport_Click);
      this.OtherLabel.Name = "OtherLabel";
      componentResourceManager.ApplyResources((object) this.OtherLabel, "OtherLabel");
      this.rbFieldOne.Name = "rbFieldOne";
      componentResourceManager.ApplyResources((object) this.rbFieldOne, "rbFieldOne");
      this.rbFieldOne.Click += new EventHandler(this.rbFieldOne_Click);
      this.rbFieldOneBySpecies.Name = "rbFieldOneBySpecies";
      componentResourceManager.ApplyResources((object) this.rbFieldOneBySpecies, "rbFieldOneBySpecies");
      this.rbFieldOneBySpecies.Click += new EventHandler(this.rbFieldOneBySpecies_Click);
      this.rbFieldOneReport.Name = "rbFieldOneReport";
      componentResourceManager.ApplyResources((object) this.rbFieldOneReport, "rbFieldOneReport");
      this.rbFieldOneReport.Click += new EventHandler(this.rbFieldOneReport_Click);
      this.rbFieldTwo.Name = "rbFieldTwo";
      componentResourceManager.ApplyResources((object) this.rbFieldTwo, "rbFieldTwo");
      this.rbFieldTwo.Click += new EventHandler(this.rbFieldTwo_Click);
      this.rbFieldTwoBySpecies.Name = "rbFieldTwoBySpecies";
      componentResourceManager.ApplyResources((object) this.rbFieldTwoBySpecies, "rbFieldTwoBySpecies");
      this.rbFieldTwoBySpecies.Click += new EventHandler(this.rbFieldTwoBySpecies_Click);
      this.rbFieldTwoReport.Name = "rbFieldTwoReport";
      componentResourceManager.ApplyResources((object) this.rbFieldTwoReport, "rbFieldTwoReport");
      this.rbFieldTwoReport.Click += new EventHandler(this.rbFieldTwoReport_Click);
      this.rbFieldThree.Name = "rbFieldThree";
      componentResourceManager.ApplyResources((object) this.rbFieldThree, "rbFieldThree");
      this.rbFieldThree.Click += new EventHandler(this.rbFieldThree_Click);
      this.rbFieldThreeBySpecies.Name = "rbFieldThreeBySpecies";
      componentResourceManager.ApplyResources((object) this.rbFieldThreeBySpecies, "rbFieldThreeBySpecies");
      this.rbFieldThreeBySpecies.Click += new EventHandler(this.rbFieldThreeBySpecies_Click);
      this.rbFieldThreeReport.Name = "rbFieldThreeReport";
      componentResourceManager.ApplyResources((object) this.rbFieldThreeReport, "rbFieldThreeReport");
      this.rbFieldThreeReport.Click += new EventHandler(this.rbFieldThreeReport_Click);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.BenefitsSummaryLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.rbBenefitsSummarySpecies);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.rbBenefitsSummaryStrataSpecies);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.CarbonStorage);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.carbonStorageOfTreesBySpeciesChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.carbonStorageOfTreesByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.AnnualCarbonSequestrationOfTreesLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.annualCarbonSequestrationOfTreesBySpeciesChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.annualCarbonSequestrationOfTreesByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.AnnualNetCarbonSequestrationOfTreesLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.EnergyEffectsLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.energyEffectsOfTreesMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.HydrologyEffectsOfTrees);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.HydrologyEffectsOfTreesBySpecies);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.HydrologyEffectsOfTreesByStratum);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.OxygenProductionOfTreesLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.oxygenProductionOfTreesByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.oxygenProductionPerUnitAreaByStrataChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.PollutionRemovalByTreesandShrubs);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.monthlyPollutantRemovalByTreesAndShrubsMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.monthlyPollutantRemovalByTreesAndShrubsChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.PollutionRemovalByGrass);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.monthlyPollutantRemovalByGrassMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.monthlyPollutantRemovalByGrassChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.BioemissionsOfTreesLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.bioemissionsOfTreesBySpeciesMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.bioemissionsOfTreesByStrataMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.UVEffectsOfTreesLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.UVEffectsOfTreesByStrata);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.AllergyIndexOfTreesLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.AllergyIndexOfTreesByStratum);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.WildLifeSuitabilityLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.WildLifeSuitabilityByPlots);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.WildLifeSuitabilityByStrata);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.ReplacementValueLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.ReplacementValueByDBHClassAndSpecies);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.ManagementCostsLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.rbManagementCostsByExpenditure);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.NetAnnualBenefitsLabel);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.rbNetAnnualBenefitsForAllTreesSample);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.FoodscapeBenefits);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.FoodscapeBenefitsofTreesMenuItem);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.LeafNutrients);
      this.rmBenefitsAndCosts.Items.Add((RibbonItem) this.LeafNutrientsofTreesMenuItem);
      this.rmBenefitsAndCosts.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.ecosystem_Large;
      this.rmBenefitsAndCosts.Name = "rmBenefitsAndCosts";
      componentResourceManager.ApplyResources((object) this.rmBenefitsAndCosts, "rmBenefitsAndCosts");
      this.BenefitsSummaryLabel.Name = "BenefitsSummaryLabel";
      componentResourceManager.ApplyResources((object) this.BenefitsSummaryLabel, "BenefitsSummaryLabel");
      this.rbBenefitsSummarySpecies.Name = "rbBenefitsSummarySpecies";
      componentResourceManager.ApplyResources((object) this.rbBenefitsSummarySpecies, "rbBenefitsSummarySpecies");
      this.rbBenefitsSummarySpecies.Click += new EventHandler(this.rbBenefitsSummaryBySpecies_Click);
      this.rbBenefitsSummaryStrataSpecies.Name = "rbBenefitsSummaryStrataSpecies";
      componentResourceManager.ApplyResources((object) this.rbBenefitsSummaryStrataSpecies, "rbBenefitsSummaryStrataSpecies");
      this.rbBenefitsSummaryStrataSpecies.Click += new EventHandler(this.rbBenefitsSummaryByStrataSpecies_Click);
      this.CarbonStorage.Name = "CarbonStorage";
      componentResourceManager.ApplyResources((object) this.CarbonStorage, "CarbonStorage");
      this.carbonStorageOfTreesBySpeciesChartMenuItem.Name = "carbonStorageOfTreesBySpeciesChartMenuItem";
      componentResourceManager.ApplyResources((object) this.carbonStorageOfTreesBySpeciesChartMenuItem, "carbonStorageOfTreesBySpeciesChartMenuItem");
      this.carbonStorageOfTreesBySpeciesChartMenuItem.Click += new EventHandler(this.carbonStorageOfTreesBySpeciesChartMenuItem_Click);
      this.carbonStorageOfTreesByStrataChartMenuItem.Name = "carbonStorageOfTreesByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.carbonStorageOfTreesByStrataChartMenuItem, "carbonStorageOfTreesByStrataChartMenuItem");
      this.carbonStorageOfTreesByStrataChartMenuItem.Click += new EventHandler(this.carbonStorageOfTreesByStrataChartMenuItem_Click);
      this.carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem.Name = "carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem, "carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem");
      this.carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem.Click += new EventHandler(this.carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem_Click);
      this.AnnualCarbonSequestrationOfTreesLabel.Name = "AnnualCarbonSequestrationOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.AnnualCarbonSequestrationOfTreesLabel, "AnnualCarbonSequestrationOfTreesLabel");
      this.annualCarbonSequestrationOfTreesBySpeciesChartMenuItem.Name = "annualCarbonSequestrationOfTreesBySpeciesChartMenuItem";
      componentResourceManager.ApplyResources((object) this.annualCarbonSequestrationOfTreesBySpeciesChartMenuItem, "annualCarbonSequestrationOfTreesBySpeciesChartMenuItem");
      this.annualCarbonSequestrationOfTreesBySpeciesChartMenuItem.Click += new EventHandler(this.annualCarbonSequestrationOfTreesBySpeciesChartMenuItem_Click);
      this.annualCarbonSequestrationOfTreesByStrataChartMenuItem.Name = "annualCarbonSequestrationOfTreesByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.annualCarbonSequestrationOfTreesByStrataChartMenuItem, "annualCarbonSequestrationOfTreesByStrataChartMenuItem");
      this.annualCarbonSequestrationOfTreesByStrataChartMenuItem.Click += new EventHandler(this.annualCarbonSequestrationOfTreesByStrataChartMenuItem_Click);
      this.annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem.Name = "annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem, "annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem");
      this.annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem.Click += new EventHandler(this.annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem_Click);
      this.AnnualNetCarbonSequestrationOfTreesLabel.Name = "AnnualNetCarbonSequestrationOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.AnnualNetCarbonSequestrationOfTreesLabel, "AnnualNetCarbonSequestrationOfTreesLabel");
      this.AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem.Name = "AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem";
      componentResourceManager.ApplyResources((object) this.AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem, "AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem");
      this.AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem.Click += new EventHandler(this.NetAnnualCarbonSequestrationOfTreesBySpeciesMenuItem_Click);
      this.AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem.Name = "AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem, "AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem");
      this.AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem.Click += new EventHandler(this.AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem_Click);
      this.AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem.Name = "AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem, "AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem");
      this.AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem.Click += new EventHandler(this.AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem_Click);
      this.EnergyEffectsLabel.Name = "EnergyEffectsLabel";
      componentResourceManager.ApplyResources((object) this.EnergyEffectsLabel, "EnergyEffectsLabel");
      this.energyEffectsOfTreesMenuItem.Name = "energyEffectsOfTreesMenuItem";
      componentResourceManager.ApplyResources((object) this.energyEffectsOfTreesMenuItem, "energyEffectsOfTreesMenuItem");
      this.energyEffectsOfTreesMenuItem.Click += new EventHandler(this.energyEffectsOfTreesMenuItem_Click);
      this.HydrologyEffectsOfTrees.Name = "HydrologyEffectsOfTrees";
      componentResourceManager.ApplyResources((object) this.HydrologyEffectsOfTrees, "HydrologyEffectsOfTrees");
      this.HydrologyEffectsOfTreesBySpecies.Name = "HydrologyEffectsOfTreesBySpecies";
      componentResourceManager.ApplyResources((object) this.HydrologyEffectsOfTreesBySpecies, "HydrologyEffectsOfTreesBySpecies");
      this.HydrologyEffectsOfTreesBySpecies.Click += new EventHandler(this.HydrologyEffectsOfTreesBySpecies_Click);
      this.HydrologyEffectsOfTreesByStratum.Name = "HydrologyEffectsOfTreesByStratum";
      componentResourceManager.ApplyResources((object) this.HydrologyEffectsOfTreesByStratum, "HydrologyEffectsOfTreesByStratum");
      this.HydrologyEffectsOfTreesByStratum.Click += new EventHandler(this.HydrologyEffectsOfTreesByStratum_Click);
      this.OxygenProductionOfTreesLabel.Name = "OxygenProductionOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.OxygenProductionOfTreesLabel, "OxygenProductionOfTreesLabel");
      this.oxygenProductionOfTreesByStrataChartMenuItem.Name = "oxygenProductionOfTreesByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.oxygenProductionOfTreesByStrataChartMenuItem, "oxygenProductionOfTreesByStrataChartMenuItem");
      this.oxygenProductionOfTreesByStrataChartMenuItem.Click += new EventHandler(this.oxygenProductionOfTreesByStrataChartMenuItem_Click);
      this.oxygenProductionPerUnitAreaByStrataChartMenuItem.Name = "oxygenProductionPerUnitAreaByStrataChartMenuItem";
      componentResourceManager.ApplyResources((object) this.oxygenProductionPerUnitAreaByStrataChartMenuItem, "oxygenProductionPerUnitAreaByStrataChartMenuItem");
      this.oxygenProductionPerUnitAreaByStrataChartMenuItem.Click += new EventHandler(this.oxygenProductionPerUnitAreaByStrataChartMenuItem_Click);
      this.PollutionRemovalByTreesandShrubs.Name = "PollutionRemovalByTreesandShrubs";
      componentResourceManager.ApplyResources((object) this.PollutionRemovalByTreesandShrubs, "PollutionRemovalByTreesandShrubs");
      this.monthlyPollutantRemovalByTreesAndShrubsMenuItem.Name = "monthlyPollutantRemovalByTreesAndShrubsMenuItem";
      componentResourceManager.ApplyResources((object) this.monthlyPollutantRemovalByTreesAndShrubsMenuItem, "monthlyPollutantRemovalByTreesAndShrubsMenuItem");
      this.monthlyPollutantRemovalByTreesAndShrubsMenuItem.Click += new EventHandler(this.monthlyPollutantRemovalByTreesAndShrubsMenuItem_Click);
      this.monthlyPollutantRemovalByTreesAndShrubsChartMenuItem.Name = "monthlyPollutantRemovalByTreesAndShrubsChartMenuItem";
      componentResourceManager.ApplyResources((object) this.monthlyPollutantRemovalByTreesAndShrubsChartMenuItem, "monthlyPollutantRemovalByTreesAndShrubsChartMenuItem");
      this.monthlyPollutantRemovalByTreesAndShrubsChartMenuItem.Click += new EventHandler(this.monthlyPollutantRemovalByTreesAndShrubsChartMenuItem_Click);
      this.PollutionRemovalByGrass.Name = "PollutionRemovalByGrass";
      componentResourceManager.ApplyResources((object) this.PollutionRemovalByGrass, "PollutionRemovalByGrass");
      this.monthlyPollutantRemovalByGrassMenuItem.Name = "monthlyPollutantRemovalByGrassMenuItem";
      componentResourceManager.ApplyResources((object) this.monthlyPollutantRemovalByGrassMenuItem, "monthlyPollutantRemovalByGrassMenuItem");
      this.monthlyPollutantRemovalByGrassMenuItem.Click += new EventHandler(this.monthlyPollutantRemovalByGrassMenuItem_Click);
      this.monthlyPollutantRemovalByGrassChartMenuItem.Name = "monthlyPollutantRemovalByGrassChartMenuItem";
      componentResourceManager.ApplyResources((object) this.monthlyPollutantRemovalByGrassChartMenuItem, "monthlyPollutantRemovalByGrassChartMenuItem");
      this.monthlyPollutantRemovalByGrassChartMenuItem.Click += new EventHandler(this.monthlyPollutantRemovalByGrassChartMenuItem_Click);
      this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem.Enabled = false;
      this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem.Name = "hourlyPollutantRemovalByTreesAndShrubsChartMenuItem";
      componentResourceManager.ApplyResources((object) this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem, "hourlyPollutantRemovalByTreesAndShrubsChartMenuItem");
      this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem.Visible = false;
      this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem.Click += new EventHandler(this.hourlyPollutantRemovalByTreesAndShrubsChartMenuItem_Click);
      this.BioemissionsOfTreesLabel.Name = "BioemissionsOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.BioemissionsOfTreesLabel, "BioemissionsOfTreesLabel");
      this.bioemissionsOfTreesBySpeciesMenuItem.Name = "bioemissionsOfTreesBySpeciesMenuItem";
      componentResourceManager.ApplyResources((object) this.bioemissionsOfTreesBySpeciesMenuItem, "bioemissionsOfTreesBySpeciesMenuItem");
      this.bioemissionsOfTreesBySpeciesMenuItem.Click += new EventHandler(this.bioemissionsOfTreesBySpeciesMenuItem_Click);
      this.bioemissionsOfTreesByStrataMenuItem.Name = "bioemissionsOfTreesByStrataMenuItem";
      componentResourceManager.ApplyResources((object) this.bioemissionsOfTreesByStrataMenuItem, "bioemissionsOfTreesByStrataMenuItem");
      this.bioemissionsOfTreesByStrataMenuItem.Click += new EventHandler(this.bioemissionsOfTreesByStrataMenuItem_Click);
      this.UVEffectsOfTreesLabel.Name = "UVEffectsOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.UVEffectsOfTreesLabel, "UVEffectsOfTreesLabel");
      this.UVEffectsOfTreesByStrata.Name = "UVEffectsOfTreesByStrata";
      componentResourceManager.ApplyResources((object) this.UVEffectsOfTreesByStrata, "UVEffectsOfTreesByStrata");
      this.UVEffectsOfTreesByStrata.Click += new EventHandler(this.UVEffectsOfTreesByStrata_Click);
      this.AllergyIndexOfTreesLabel.Name = "AllergyIndexOfTreesLabel";
      componentResourceManager.ApplyResources((object) this.AllergyIndexOfTreesLabel, "AllergyIndexOfTreesLabel");
      this.AllergyIndexOfTreesByStratum.Name = "AllergyIndexOfTreesByStratum";
      componentResourceManager.ApplyResources((object) this.AllergyIndexOfTreesByStratum, "AllergyIndexOfTreesByStratum");
      this.AllergyIndexOfTreesByStratum.Click += new EventHandler(this.AllergyIndexOfTreesByStrata_Click);
      this.WildLifeSuitabilityLabel.Name = "WildLifeSuitabilityLabel";
      componentResourceManager.ApplyResources((object) this.WildLifeSuitabilityLabel, "WildLifeSuitabilityLabel");
      this.WildLifeSuitabilityByPlots.Name = "WildLifeSuitabilityByPlots";
      componentResourceManager.ApplyResources((object) this.WildLifeSuitabilityByPlots, "WildLifeSuitabilityByPlots");
      this.WildLifeSuitabilityByPlots.Click += new EventHandler(this.WildLifeSuitabilityByPlots_Click);
      this.WildLifeSuitabilityByStrata.Name = "WildLifeSuitabilityByStrata";
      componentResourceManager.ApplyResources((object) this.WildLifeSuitabilityByStrata, "WildLifeSuitabilityByStrata");
      this.WildLifeSuitabilityByStrata.Click += new EventHandler(this.WildLifeSuitabilityByStrata_Click);
      this.ReplacementValueLabel.Enabled = false;
      this.ReplacementValueLabel.Name = "ReplacementValueLabel";
      componentResourceManager.ApplyResources((object) this.ReplacementValueLabel, "ReplacementValueLabel");
      this.ReplacementValueLabel.Visible = false;
      this.ReplacementValueByDBHClassAndSpecies.Enabled = false;
      this.ReplacementValueByDBHClassAndSpecies.Name = "ReplacementValueByDBHClassAndSpecies";
      componentResourceManager.ApplyResources((object) this.ReplacementValueByDBHClassAndSpecies, "ReplacementValueByDBHClassAndSpecies");
      this.ReplacementValueByDBHClassAndSpecies.Visible = false;
      this.ReplacementValueByDBHClassAndSpecies.Click += new EventHandler(this.ReplacementValueByDBHClassAndSpecies_Click);
      this.ManagementCostsLabel.Name = "ManagementCostsLabel";
      componentResourceManager.ApplyResources((object) this.ManagementCostsLabel, "ManagementCostsLabel");
      this.rbManagementCostsByExpenditure.Name = "rbManagementCostsByExpenditure";
      componentResourceManager.ApplyResources((object) this.rbManagementCostsByExpenditure, "rbManagementCostsByExpenditure");
      this.rbManagementCostsByExpenditure.Click += new EventHandler(this.rbManagementCostsByExpenditure_Click);
      this.NetAnnualBenefitsLabel.Name = "NetAnnualBenefitsLabel";
      componentResourceManager.ApplyResources((object) this.NetAnnualBenefitsLabel, "NetAnnualBenefitsLabel");
      this.rbNetAnnualBenefitsForAllTreesSample.Name = "rbNetAnnualBenefitsForAllTreesSample";
      componentResourceManager.ApplyResources((object) this.rbNetAnnualBenefitsForAllTreesSample, "rbNetAnnualBenefitsForAllTreesSample");
      this.rbNetAnnualBenefitsForAllTreesSample.Click += new EventHandler(this.rbNetAnnualBenefitsForAllTreesSample_Click);
      this.FoodscapeBenefits.Name = "FoodscapeBenefits";
      componentResourceManager.ApplyResources((object) this.FoodscapeBenefits, "FoodscapeBenefits");
      this.FoodscapeBenefitsofTreesMenuItem.Name = "FoodscapeBenefitsofTreesMenuItem";
      componentResourceManager.ApplyResources((object) this.FoodscapeBenefitsofTreesMenuItem, "FoodscapeBenefitsofTreesMenuItem");
      this.FoodscapeBenefitsofTreesMenuItem.Click += new EventHandler(this.FoodscapeBenefitsOfTreesBySpecies_Click);
      this.LeafNutrients.Name = "LeafNutrients";
      componentResourceManager.ApplyResources((object) this.LeafNutrients, "LeafNutrients");
      this.LeafNutrientsofTreesMenuItem.Name = "LeafNutrientsofTreesMenuItem";
      componentResourceManager.ApplyResources((object) this.LeafNutrientsofTreesMenuItem, "LeafNutrientsofTreesMenuItem");
      this.LeafNutrientsofTreesMenuItem.Click += new EventHandler(this.LeafNutrientsofTreesMenuItem_Click);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rlMDDCompositionAndStructure);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDCompositionOfPlots);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDCompositionOfTrees);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDCompositionOfTreeBySpecies);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDCompositionOfTreesByStratum);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rlMDDTreeBenefitsAndCosts);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsSummary);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsCarbonStorage);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsCarbonSequestration);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsEnergyEffects);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeEnergyAvoidedPollutants);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsHydrologyEffects);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsPollutionRemoval);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsOxygenProduction);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDTreeBenefitsVOCEmissions);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDWoodProducts);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rlMDDMiscellaneous);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDMiscPlotComments);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDMiscTreeComments);
      this.rmIndividualLevelResults.Items.Add((RibbonItem) this.rbMDDMiscShrubComments);
      this.rmIndividualLevelResults.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.individual_tree_Large;
      this.rmIndividualLevelResults.Name = "rmIndividualLevelResults";
      componentResourceManager.ApplyResources((object) this.rmIndividualLevelResults, "rmIndividualLevelResults");
      this.rlMDDCompositionAndStructure.Name = "rlMDDCompositionAndStructure";
      componentResourceManager.ApplyResources((object) this.rlMDDCompositionAndStructure, "rlMDDCompositionAndStructure");
      this.rbMDDCompositionOfPlots.Name = "rbMDDCompositionOfPlots";
      componentResourceManager.ApplyResources((object) this.rbMDDCompositionOfPlots, "rbMDDCompositionOfPlots");
      this.rbMDDCompositionOfPlots.Click += new EventHandler(this.rbMDDCompositionOfPlots_Click);
      this.rbMDDCompositionOfTrees.Name = "rbMDDCompositionOfTrees";
      componentResourceManager.ApplyResources((object) this.rbMDDCompositionOfTrees, "rbMDDCompositionOfTrees");
      this.rbMDDCompositionOfTrees.Click += new EventHandler(this.rbMDDCompositionOfTrees_Click);
      this.rbMDDCompositionOfTreeBySpecies.Name = "rbMDDCompositionOfTreeBySpecies";
      componentResourceManager.ApplyResources((object) this.rbMDDCompositionOfTreeBySpecies, "rbMDDCompositionOfTreeBySpecies");
      this.rbMDDCompositionOfTreeBySpecies.Click += new EventHandler(this.rbMDDCompositionOfTreeBySpecies_Click);
      this.rbMDDCompositionOfTreesByStratum.Name = "rbMDDCompositionOfTreesByStratum";
      componentResourceManager.ApplyResources((object) this.rbMDDCompositionOfTreesByStratum, "rbMDDCompositionOfTreesByStratum");
      this.rbMDDCompositionOfTreesByStratum.Click += new EventHandler(this.rbMDDCompositionOfTreesByStratum_Click);
      this.rlMDDTreeBenefitsAndCosts.Name = "rlMDDTreeBenefitsAndCosts";
      componentResourceManager.ApplyResources((object) this.rlMDDTreeBenefitsAndCosts, "rlMDDTreeBenefitsAndCosts");
      this.rbMDDTreeBenefitsSummary.Name = "rbMDDTreeBenefitsSummary";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsSummary, "rbMDDTreeBenefitsSummary");
      this.rbMDDTreeBenefitsSummary.Click += new EventHandler(this.rbMDDTreeBenefitsSummary_Click);
      this.rbMDDTreeBenefitsCarbonStorage.Name = "rbMDDTreeBenefitsCarbonStorage";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsCarbonStorage, "rbMDDTreeBenefitsCarbonStorage");
      this.rbMDDTreeBenefitsCarbonStorage.Click += new EventHandler(this.rbMDDTreeBenefitsCarbonStorage_Click);
      this.rbMDDTreeBenefitsCarbonSequestration.Name = "rbMDDTreeBenefitsCarbonSequestration";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsCarbonSequestration, "rbMDDTreeBenefitsCarbonSequestration");
      this.rbMDDTreeBenefitsCarbonSequestration.Click += new EventHandler(this.rbMDDTreeBenefitsCarbonSequestration_Click);
      this.rbMDDTreeBenefitsEnergyEffects.Name = "rbMDDTreeBenefitsEnergyEffects";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsEnergyEffects, "rbMDDTreeBenefitsEnergyEffects");
      this.rbMDDTreeBenefitsEnergyEffects.Click += new EventHandler(this.rbMDDTreeBenefitsEnergyEffects_Click);
      this.rbMDDTreeEnergyAvoidedPollutants.Name = "rbMDDTreeEnergyAvoidedPollutants";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeEnergyAvoidedPollutants, "rbMDDTreeEnergyAvoidedPollutants");
      this.rbMDDTreeEnergyAvoidedPollutants.Click += new EventHandler(this.rbMDDTreeEnergyAvoidedPollutants_Click);
      this.rbMDDTreeBenefitsHydrologyEffects.Name = "rbMDDTreeBenefitsHydrologyEffects";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsHydrologyEffects, "rbMDDTreeBenefitsHydrologyEffects");
      this.rbMDDTreeBenefitsHydrologyEffects.Click += new EventHandler(this.rbMDDTreeBenefitsHydrologyEffects_Click);
      this.rbMDDTreeBenefitsPollutionRemoval.Name = "rbMDDTreeBenefitsPollutionRemoval";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsPollutionRemoval, "rbMDDTreeBenefitsPollutionRemoval");
      this.rbMDDTreeBenefitsPollutionRemoval.Click += new EventHandler(this.rbMDDTreeBenefitsPollutionRemoval_Click);
      this.rbMDDTreeBenefitsOxygenProduction.Name = "rbMDDTreeBenefitsOxygenProduction";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsOxygenProduction, "rbMDDTreeBenefitsOxygenProduction");
      this.rbMDDTreeBenefitsOxygenProduction.Click += new EventHandler(this.rbMDDTreeBenefitsOxygenProduction_Click);
      this.rbMDDTreeBenefitsVOCEmissions.Name = "rbMDDTreeBenefitsVOCEmissions";
      componentResourceManager.ApplyResources((object) this.rbMDDTreeBenefitsVOCEmissions, "rbMDDTreeBenefitsVOCEmissions");
      this.rbMDDTreeBenefitsVOCEmissions.Click += new EventHandler(this.rbMDDTreeBenefitsVOCEmissions_Click);
      this.rbMDDWoodProducts.Name = "rbMDDWoodProducts";
      componentResourceManager.ApplyResources((object) this.rbMDDWoodProducts, "rbMDDWoodProducts");
      this.rbMDDWoodProducts.Click += new EventHandler(this.rbMDDWoodProducts_Click);
      this.rlMDDMiscellaneous.Name = "rlMDDMiscellaneous";
      componentResourceManager.ApplyResources((object) this.rlMDDMiscellaneous, "rlMDDMiscellaneous");
      this.rbMDDMiscPlotComments.Name = "rbMDDMiscPlotComments";
      componentResourceManager.ApplyResources((object) this.rbMDDMiscPlotComments, "rbMDDMiscPlotComments");
      this.rbMDDMiscPlotComments.Click += new EventHandler(this.rbMDDMiscPlotComments_Click);
      this.rbMDDMiscTreeComments.Name = "rbMDDMiscTreeComments";
      componentResourceManager.ApplyResources((object) this.rbMDDMiscTreeComments, "rbMDDMiscTreeComments");
      this.rbMDDMiscTreeComments.Click += new EventHandler(this.rbMDDMiscTreeComments_Click);
      this.rbMDDMiscShrubComments.Name = "rbMDDMiscShrubComments";
      componentResourceManager.ApplyResources((object) this.rbMDDMiscShrubComments, "rbMDDMiscShrubComments");
      this.rbMDDMiscShrubComments.Click += new EventHandler(this.rbMDDMiscShrubComments_Click);
      this.rmAirQualityHealthImpactsAndValuesReports.Items.Add((RibbonItem) this.airQualityHealthImpactsandValuesbyTrees);
      this.rmAirQualityHealthImpactsAndValuesReports.Items.Add((RibbonItem) this.airQualityHealthImpactsandValuesbyShrubs);
      this.rmAirQualityHealthImpactsAndValuesReports.Items.Add((RibbonItem) this.airQualityHealthImpactsandValuesbyGrass);
      this.rmAirQualityHealthImpactsAndValuesReports.Items.Add((RibbonItem) this.airQualityHealthImpactsandValuesCombined);
      this.rmAirQualityHealthImpactsAndValuesReports.Items.Add((RibbonItem) this.airQualityHealthImpactsandValuesbyTreesandShrubs);
      this.rmAirQualityHealthImpactsAndValuesReports.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.air_quality_Large;
      this.rmAirQualityHealthImpactsAndValuesReports.Name = "rmAirQualityHealthImpactsAndValuesReports";
      componentResourceManager.ApplyResources((object) this.rmAirQualityHealthImpactsAndValuesReports, "rmAirQualityHealthImpactsAndValuesReports");
      this.airQualityHealthImpactsandValuesbyTrees.Name = "airQualityHealthImpactsandValuesbyTrees";
      componentResourceManager.ApplyResources((object) this.airQualityHealthImpactsandValuesbyTrees, "airQualityHealthImpactsandValuesbyTrees");
      this.airQualityHealthImpactsandValuesbyTrees.Click += new EventHandler(this.airQualityHealthImpactsandValuesbyTrees_Click);
      this.airQualityHealthImpactsandValuesbyShrubs.Name = "airQualityHealthImpactsandValuesbyShrubs";
      componentResourceManager.ApplyResources((object) this.airQualityHealthImpactsandValuesbyShrubs, "airQualityHealthImpactsandValuesbyShrubs");
      this.airQualityHealthImpactsandValuesbyShrubs.Click += new EventHandler(this.airQualityHealthImpactsandValuesbyShrubs_Click);
      this.airQualityHealthImpactsandValuesbyGrass.Name = "airQualityHealthImpactsandValuesbyGrass";
      componentResourceManager.ApplyResources((object) this.airQualityHealthImpactsandValuesbyGrass, "airQualityHealthImpactsandValuesbyGrass");
      this.airQualityHealthImpactsandValuesbyGrass.Click += new EventHandler(this.airQualityHealthImpactsandValuesbyGrass_Click);
      this.airQualityHealthImpactsandValuesCombined.Name = "airQualityHealthImpactsandValuesCombined";
      componentResourceManager.ApplyResources((object) this.airQualityHealthImpactsandValuesCombined, "airQualityHealthImpactsandValuesCombined");
      this.airQualityHealthImpactsandValuesCombined.Click += new EventHandler(this.airQualityHealthImpactsandValuesCombined_Click);
      this.airQualityHealthImpactsandValuesbyTreesandShrubs.Name = "airQualityHealthImpactsandValuesbyTreesandShrubs";
      componentResourceManager.ApplyResources((object) this.airQualityHealthImpactsandValuesbyTreesandShrubs, "airQualityHealthImpactsandValuesbyTreesandShrubs");
      this.airQualityHealthImpactsandValuesbyTreesandShrubs.Click += new EventHandler(this.airQualityHealthImpactsandValuesbyTreesandShrubs_Click);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.SusceptibilityOfTreesToPestsLabel);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.SusceptibilityOfTreesToPestsByStrata);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.PrimaryPestLabel);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.primaryPestSummaryOfTreesByStrataButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.primaryPestDetailsOfTreesByStrataButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.SignAndSymptomLabel);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.signSymptomOverviewBySpeciesButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.signSymptomDetailsSummaryBySpeciesButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.signSymptomDetailsCompleteBySpeciesButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.signSymptomOverviewByStrataButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.signSymptomDetailsSummaryByStrataButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.signSymptomDetailsCompleteByStrataButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.signSymptomReviewOfTreesButton);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.PestReviewLabel);
      this.rmPestAnalysis.Items.Add((RibbonItem) this.pestReviewOfTreesButton);
      this.rmPestAnalysis.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.pest_analysis_Large;
      this.rmPestAnalysis.Name = "rmPestAnalysis";
      componentResourceManager.ApplyResources((object) this.rmPestAnalysis, "rmPestAnalysis");
      this.SusceptibilityOfTreesToPestsLabel.Name = "SusceptibilityOfTreesToPestsLabel";
      componentResourceManager.ApplyResources((object) this.SusceptibilityOfTreesToPestsLabel, "SusceptibilityOfTreesToPestsLabel");
      this.SusceptibilityOfTreesToPestsByStrata.Name = "SusceptibilityOfTreesToPestsByStrata";
      componentResourceManager.ApplyResources((object) this.SusceptibilityOfTreesToPestsByStrata, "SusceptibilityOfTreesToPestsByStrata");
      this.SusceptibilityOfTreesToPestsByStrata.Click += new EventHandler(this.SusceptibilityOfTreesToPestsByStrata_Click);
      this.PrimaryPestLabel.Name = "PrimaryPestLabel";
      componentResourceManager.ApplyResources((object) this.PrimaryPestLabel, "PrimaryPestLabel");
      this.primaryPestSummaryOfTreesByStrataButton.Name = "primaryPestSummaryOfTreesByStrataButton";
      componentResourceManager.ApplyResources((object) this.primaryPestSummaryOfTreesByStrataButton, "primaryPestSummaryOfTreesByStrataButton");
      this.primaryPestSummaryOfTreesByStrataButton.Click += new EventHandler(this.primaryPestSummaryOfTreesByStrataButton_Click);
      this.primaryPestDetailsOfTreesByStrataButton.Name = "primaryPestDetailsOfTreesByStrataButton";
      componentResourceManager.ApplyResources((object) this.primaryPestDetailsOfTreesByStrataButton, "primaryPestDetailsOfTreesByStrataButton");
      this.primaryPestDetailsOfTreesByStrataButton.Click += new EventHandler(this.primaryPestDetailsOfTreesByStrataButton_Click);
      this.SignAndSymptomLabel.Name = "SignAndSymptomLabel";
      componentResourceManager.ApplyResources((object) this.SignAndSymptomLabel, "SignAndSymptomLabel");
      this.signSymptomOverviewBySpeciesButton.Name = "signSymptomOverviewBySpeciesButton";
      componentResourceManager.ApplyResources((object) this.signSymptomOverviewBySpeciesButton, "signSymptomOverviewBySpeciesButton");
      this.signSymptomOverviewBySpeciesButton.Click += new EventHandler(this.signSymptomOverviewBySpeciesButton_Click);
      this.signSymptomDetailsSummaryBySpeciesButton.Name = "signSymptomDetailsSummaryBySpeciesButton";
      componentResourceManager.ApplyResources((object) this.signSymptomDetailsSummaryBySpeciesButton, "signSymptomDetailsSummaryBySpeciesButton");
      this.signSymptomDetailsSummaryBySpeciesButton.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageAboveText;
      this.signSymptomDetailsSummaryBySpeciesButton.Click += new EventHandler(this.signSymptomDetailsSummaryBySpeciesButton_Click);
      this.signSymptomDetailsCompleteBySpeciesButton.Name = "signSymptomDetailsCompleteBySpeciesButton";
      componentResourceManager.ApplyResources((object) this.signSymptomDetailsCompleteBySpeciesButton, "signSymptomDetailsCompleteBySpeciesButton");
      this.signSymptomDetailsCompleteBySpeciesButton.Click += new EventHandler(this.signSymptomDetailsCompleteBySpeciesButton_Click);
      this.signSymptomOverviewByStrataButton.Name = "signSymptomOverviewByStrataButton";
      componentResourceManager.ApplyResources((object) this.signSymptomOverviewByStrataButton, "signSymptomOverviewByStrataButton");
      this.signSymptomOverviewByStrataButton.Click += new EventHandler(this.signSymptomOverviewByStrataButton_Click);
      this.signSymptomDetailsSummaryByStrataButton.Name = "signSymptomDetailsSummaryByStrataButton";
      componentResourceManager.ApplyResources((object) this.signSymptomDetailsSummaryByStrataButton, "signSymptomDetailsSummaryByStrataButton");
      this.signSymptomDetailsSummaryByStrataButton.Click += new EventHandler(this.signSymptomDetailsSummaryByStrataButton_Click);
      this.signSymptomDetailsCompleteByStrataButton.Name = "signSymptomDetailsCompleteByStrataButton";
      componentResourceManager.ApplyResources((object) this.signSymptomDetailsCompleteByStrataButton, "signSymptomDetailsCompleteByStrataButton");
      this.signSymptomDetailsCompleteByStrataButton.Click += new EventHandler(this.signSymptomDetailsCompleteByStrataButton_Click);
      this.signSymptomReviewOfTreesButton.Name = "signSymptomReviewOfTreesButton";
      componentResourceManager.ApplyResources((object) this.signSymptomReviewOfTreesButton, "signSymptomReviewOfTreesButton");
      this.signSymptomReviewOfTreesButton.Click += new EventHandler(this.signSymptomReviewOfTreesButton_Click);
      this.PestReviewLabel.Name = "PestReviewLabel";
      componentResourceManager.ApplyResources((object) this.PestReviewLabel, "PestReviewLabel");
      this.pestReviewOfTreesButton.Name = "pestReviewOfTreesButton";
      componentResourceManager.ApplyResources((object) this.pestReviewOfTreesButton, "pestReviewOfTreesButton");
      this.pestReviewOfTreesButton.Click += new EventHandler(this.pestReviewOfTreesButton_Click);
      this.rgCharts.Items.Add((RibbonItem) this.rmWeatherReports);
      this.rgCharts.Name = "rgCharts";
      componentResourceManager.ApplyResources((object) this.rgCharts, "rgCharts");
      this.rmWeatherReports.Items.Add((RibbonItem) this.rRawDataLabel);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherAirPollutantConcentrationUGM3);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherPhotosyntheticallyActiveRadiation);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherRain);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherTempF);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbUVIndex);
      this.rmWeatherReports.Items.Add((RibbonItem) this.AirQualityImprovementLabel);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherAirQualImprovementTree);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherAirQualImprovementShrub);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherAirQualImprovementGrass);
      this.rmWeatherReports.Items.Add((RibbonItem) this.AirPollutantFluxDryDepositionLabel);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherDryDepTree);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherDryDepShrub);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWeatherDryDepGrass);
      this.rmWeatherReports.Items.Add((RibbonItem) this.TranspirationLabel);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbTranspirationByTree);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbTranspirationByShrub);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rlEvaporation);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbEvaporationByTrees);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbEvaporationByShrubs);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rlWaterInterception);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWaterInterceptionByTrees);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbWaterInterceptionByShrubs);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rlAvoidedRunoff);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbAvoidedRunoffByTrees);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbAvoidedRunoffByShrubs);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbPotentialEvapotranspiration);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbPotentialEvapotranspirationByTrees);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbPotentialEvapotranspirationByShrubs);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbUVIndexReductionLabel);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbUVIndexReductionByTreesForUnderTrees);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbUVIndexReductionByTreesForOverall);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbIsoprene);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbIsopreneByTrees);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbIsopreneByShrubs);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbMonoterpene);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbMonoterpeneByTrees);
      this.rmWeatherReports.Items.Add((RibbonItem) this.rbMonoterpeneByShrubs);
      this.rmWeatherReports.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Cloud_Lightningbolt;
      this.rmWeatherReports.Name = "rmWeatherReports";
      componentResourceManager.ApplyResources((object) this.rmWeatherReports, "rmWeatherReports");
      this.rRawDataLabel.Name = "rRawDataLabel";
      componentResourceManager.ApplyResources((object) this.rRawDataLabel, "rRawDataLabel");
      this.rbWeatherAirPollutantConcentrationUGM3.Name = "rbWeatherAirPollutantConcentrationUGM3";
      componentResourceManager.ApplyResources((object) this.rbWeatherAirPollutantConcentrationUGM3, "rbWeatherAirPollutantConcentrationUGM3");
      this.rbWeatherAirPollutantConcentrationUGM3.Click += new EventHandler(this.rbWeatherAirPollutantConcentrationUGM3_Click);
      this.rbWeatherPhotosyntheticallyActiveRadiation.Name = "rbWeatherPhotosyntheticallyActiveRadiation";
      componentResourceManager.ApplyResources((object) this.rbWeatherPhotosyntheticallyActiveRadiation, "rbWeatherPhotosyntheticallyActiveRadiation");
      this.rbWeatherPhotosyntheticallyActiveRadiation.Click += new EventHandler(this.rbWeatherPhotosyntheticallyActiveRadiation_Click);
      this.rbWeatherRain.Name = "rbWeatherRain";
      componentResourceManager.ApplyResources((object) this.rbWeatherRain, "rbWeatherRain");
      this.rbWeatherRain.Click += new EventHandler(this.rbWeatherRain_Click);
      this.rbWeatherTempF.Name = "rbWeatherTempF";
      componentResourceManager.ApplyResources((object) this.rbWeatherTempF, "rbWeatherTempF");
      this.rbWeatherTempF.Click += new EventHandler(this.rbWeatherTempF_Click);
      this.rbUVIndex.Name = "rbUVIndex";
      componentResourceManager.ApplyResources((object) this.rbUVIndex, "rbUVIndex");
      this.rbUVIndex.Click += new EventHandler(this.rbUVIndex_Click);
      this.AirQualityImprovementLabel.Name = "AirQualityImprovementLabel";
      componentResourceManager.ApplyResources((object) this.AirQualityImprovementLabel, "AirQualityImprovementLabel");
      this.rbWeatherAirQualImprovementTree.Name = "rbWeatherAirQualImprovementTree";
      componentResourceManager.ApplyResources((object) this.rbWeatherAirQualImprovementTree, "rbWeatherAirQualImprovementTree");
      this.rbWeatherAirQualImprovementTree.Click += new EventHandler(this.rbWeatherAirQualImprovementTree_Click);
      this.rbWeatherAirQualImprovementShrub.Name = "rbWeatherAirQualImprovementShrub";
      componentResourceManager.ApplyResources((object) this.rbWeatherAirQualImprovementShrub, "rbWeatherAirQualImprovementShrub");
      this.rbWeatherAirQualImprovementShrub.Click += new EventHandler(this.rbWeatherAirQualImprovementShrub_Click);
      this.rbWeatherAirQualImprovementGrass.Name = "rbWeatherAirQualImprovementGrass";
      componentResourceManager.ApplyResources((object) this.rbWeatherAirQualImprovementGrass, "rbWeatherAirQualImprovementGrass");
      this.rbWeatherAirQualImprovementGrass.Click += new EventHandler(this.rbWeatherAirQualImprovementGrass_Click);
      this.AirPollutantFluxDryDepositionLabel.Name = "AirPollutantFluxDryDepositionLabel";
      componentResourceManager.ApplyResources((object) this.AirPollutantFluxDryDepositionLabel, "AirPollutantFluxDryDepositionLabel");
      this.rbWeatherDryDepTree.Name = "rbWeatherDryDepTree";
      componentResourceManager.ApplyResources((object) this.rbWeatherDryDepTree, "rbWeatherDryDepTree");
      this.rbWeatherDryDepTree.Click += new EventHandler(this.rbWeatherDryDepTree_Click);
      this.rbWeatherDryDepShrub.Name = "rbWeatherDryDepShrub";
      componentResourceManager.ApplyResources((object) this.rbWeatherDryDepShrub, "rbWeatherDryDepShrub");
      this.rbWeatherDryDepShrub.Click += new EventHandler(this.rbWeatherDryDepShrub_Click);
      this.rbWeatherDryDepGrass.Name = "rbWeatherDryDepGrass";
      componentResourceManager.ApplyResources((object) this.rbWeatherDryDepGrass, "rbWeatherDryDepGrass");
      this.rbWeatherDryDepGrass.Click += new EventHandler(this.rbWeatherDryDepGrass_Click);
      this.TranspirationLabel.Name = "TranspirationLabel";
      componentResourceManager.ApplyResources((object) this.TranspirationLabel, "TranspirationLabel");
      this.rbTranspirationByTree.Name = "rbTranspirationByTree";
      componentResourceManager.ApplyResources((object) this.rbTranspirationByTree, "rbTranspirationByTree");
      this.rbTranspirationByTree.Click += new EventHandler(this.rbTranspirationByTree_Click);
      this.rbTranspirationByShrub.Name = "rbTranspirationByShrub";
      componentResourceManager.ApplyResources((object) this.rbTranspirationByShrub, "rbTranspirationByShrub");
      this.rbTranspirationByShrub.Click += new EventHandler(this.rbTranspirationByShrub_Click);
      this.rlEvaporation.Name = "rlEvaporation";
      componentResourceManager.ApplyResources((object) this.rlEvaporation, "rlEvaporation");
      this.rbEvaporationByTrees.Name = "rbEvaporationByTrees";
      componentResourceManager.ApplyResources((object) this.rbEvaporationByTrees, "rbEvaporationByTrees");
      this.rbEvaporationByTrees.Click += new EventHandler(this.rbEvaporationByTrees_Click);
      this.rbEvaporationByShrubs.Name = "rbEvaporationByShrubs";
      componentResourceManager.ApplyResources((object) this.rbEvaporationByShrubs, "rbEvaporationByShrubs");
      this.rbEvaporationByShrubs.Click += new EventHandler(this.rbEvaporationByShrubs_Click);
      this.rlWaterInterception.Name = "rlWaterInterception";
      componentResourceManager.ApplyResources((object) this.rlWaterInterception, "rlWaterInterception");
      this.rbWaterInterceptionByTrees.Name = "rbWaterInterceptionByTrees";
      componentResourceManager.ApplyResources((object) this.rbWaterInterceptionByTrees, "rbWaterInterceptionByTrees");
      this.rbWaterInterceptionByTrees.Click += new EventHandler(this.rbWaterInterceptionByTrees_Click);
      this.rbWaterInterceptionByShrubs.Name = "rbWaterInterceptionByShrubs";
      componentResourceManager.ApplyResources((object) this.rbWaterInterceptionByShrubs, "rbWaterInterceptionByShrubs");
      this.rbWaterInterceptionByShrubs.Click += new EventHandler(this.rbWaterInterceptionByShrubs_Click);
      this.rlAvoidedRunoff.Name = "rlAvoidedRunoff";
      componentResourceManager.ApplyResources((object) this.rlAvoidedRunoff, "rlAvoidedRunoff");
      this.rbAvoidedRunoffByTrees.Name = "rbAvoidedRunoffByTrees";
      componentResourceManager.ApplyResources((object) this.rbAvoidedRunoffByTrees, "rbAvoidedRunoffByTrees");
      this.rbAvoidedRunoffByTrees.Click += new EventHandler(this.rbAvoidedRunoffByTrees_Click);
      this.rbAvoidedRunoffByShrubs.Name = "rbAvoidedRunoffByShrubs";
      componentResourceManager.ApplyResources((object) this.rbAvoidedRunoffByShrubs, "rbAvoidedRunoffByShrubs");
      this.rbAvoidedRunoffByShrubs.Click += new EventHandler(this.rbAvoidedRunoffByShrubs_Click);
      this.rbPotentialEvapotranspiration.Name = "rbPotentialEvapotranspiration";
      componentResourceManager.ApplyResources((object) this.rbPotentialEvapotranspiration, "rbPotentialEvapotranspiration");
      this.rbPotentialEvapotranspirationByTrees.Name = "rbPotentialEvapotranspirationByTrees";
      componentResourceManager.ApplyResources((object) this.rbPotentialEvapotranspirationByTrees, "rbPotentialEvapotranspirationByTrees");
      this.rbPotentialEvapotranspirationByTrees.Click += new EventHandler(this.rbPotentialEvaporationByTrees_Click);
      this.rbPotentialEvapotranspirationByShrubs.Name = "rbPotentialEvapotranspirationByShrubs";
      componentResourceManager.ApplyResources((object) this.rbPotentialEvapotranspirationByShrubs, "rbPotentialEvapotranspirationByShrubs");
      this.rbPotentialEvapotranspirationByShrubs.Click += new EventHandler(this.rbPotentialEvaporationByShrubs_Click);
      this.rbUVIndexReductionLabel.Name = "rbUVIndexReductionLabel";
      componentResourceManager.ApplyResources((object) this.rbUVIndexReductionLabel, "rbUVIndexReductionLabel");
      this.rbUVIndexReductionByTreesForUnderTrees.Name = "rbUVIndexReductionByTreesForUnderTrees";
      componentResourceManager.ApplyResources((object) this.rbUVIndexReductionByTreesForUnderTrees, "rbUVIndexReductionByTreesForUnderTrees");
      this.rbUVIndexReductionByTreesForUnderTrees.Click += new EventHandler(this.rbUVIndexReductionByTreesForUnderTrees_Click);
      this.rbUVIndexReductionByTreesForOverall.Name = "rbUVIndexReductionByTreesForOverall";
      componentResourceManager.ApplyResources((object) this.rbUVIndexReductionByTreesForOverall, "rbUVIndexReductionByTreesForOverall");
      this.rbUVIndexReductionByTreesForOverall.Click += new EventHandler(this.rbUVIndexReductionByTreesForOverall_Click);
      this.rbIsoprene.Name = "rbIsoprene";
      componentResourceManager.ApplyResources((object) this.rbIsoprene, "rbIsoprene");
      this.rbIsopreneByTrees.Name = "rbIsopreneByTrees";
      componentResourceManager.ApplyResources((object) this.rbIsopreneByTrees, "rbIsopreneByTrees");
      this.rbIsopreneByTrees.Click += new EventHandler(this.rbIsopreneByTrees_Click);
      this.rbIsopreneByShrubs.Name = "rbIsopreneByShrubs";
      componentResourceManager.ApplyResources((object) this.rbIsopreneByShrubs, "rbIsopreneByShrubs");
      this.rbIsopreneByShrubs.Click += new EventHandler(this.rbIsopreneByShrubs_Click);
      this.rbMonoterpene.Name = "rbMonoterpene";
      componentResourceManager.ApplyResources((object) this.rbMonoterpene, "rbMonoterpene");
      this.rbMonoterpeneByTrees.Name = "rbMonoterpeneByTrees";
      componentResourceManager.ApplyResources((object) this.rbMonoterpeneByTrees, "rbMonoterpeneByTrees");
      this.rbMonoterpeneByTrees.Click += new EventHandler(this.rbMonoterpeneByTrees_Click);
      this.rbMonoterpeneByShrubs.Name = "rbMonoterpeneByShrubs";
      componentResourceManager.ApplyResources((object) this.rbMonoterpeneByShrubs, "rbMonoterpeneByShrubs");
      this.rbMonoterpeneByShrubs.Click += new EventHandler(this.rbMonoterpeneByShrubs_Click);
      this.rgReportTree.Items.Add((RibbonItem) this.ReportTreeViewToggle);
      this.rgReportTree.Name = "rgReportTree";
      this.ReportTreeViewToggle.LargeImage = (Image) componentResourceManager.GetObject("ReportTreeViewToggle.LargeImage");
      this.ReportTreeViewToggle.Name = "ReportTreeViewToggle";
      componentResourceManager.ApplyResources((object) this.ReportTreeViewToggle, "ReportTreeViewToggle");
      this.rgSettings.Items.Add((RibbonItem) this.rbReportsEnglish);
      this.rgSettings.Items.Add((RibbonItem) this.rbReportsMetric);
      this.rgSettings.Items.Add((RibbonItem) this.ribbonSeparator4);
      this.rgSettings.Items.Add((RibbonItem) this.rbReportsSpeciesCN);
      this.rgSettings.Items.Add((RibbonItem) this.rbReportsSpeciesSN);
      this.rgSettings.Items.Add((RibbonItem) this.ribbonSeparator5);
      this.rgSettings.Items.Add((RibbonItem) this.rbCheckBoxGPS);
      this.rgSettings.Items.Add((RibbonItem) this.rbCheckBoxComments);
      this.rgSettings.Items.Add((RibbonItem) this.rbCheckBoxUID);
      this.rgSettings.Items.Add((RibbonItem) this.rbCheckBoxHideZeros);
      this.rgSettings.Name = "rgSettings";
      componentResourceManager.ApplyResources((object) this.rgSettings, "rgSettings");
      this.rbReportsEnglish.CanDepress = false;
      this.rbReportsEnglish.Name = "rbReportsEnglish";
      this.rbReportsEnglish.Pressed = true;
      componentResourceManager.ApplyResources((object) this.rbReportsEnglish, "rbReportsEnglish");
      this.rbReportsEnglish.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbReportsEnglish.ToggleGroupName = "tgUnit";
      this.rbReportsEnglish.PressedChanged += new EventHandler(this.rbReportsUnits_PressedChanged);
      this.rbReportsEnglish.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgUnits_MeasureItem);
      this.rbReportsEnglish.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgUnits_DrawItem);
      this.rbReportsMetric.CanDepress = false;
      this.rbReportsMetric.Name = "rbReportsMetric";
      componentResourceManager.ApplyResources((object) this.rbReportsMetric, "rbReportsMetric");
      this.rbReportsMetric.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbReportsMetric.ToggleGroupName = "tgUnit";
      this.rbReportsMetric.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgUnits_MeasureItem);
      this.rbReportsMetric.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgUnits_DrawItem);
      this.ribbonSeparator4.Name = "ribbonSeparator4";
      this.rbReportsSpeciesCN.CanDepress = false;
      this.rbReportsSpeciesCN.Name = "rbReportsSpeciesCN";
      this.rbReportsSpeciesCN.Pressed = true;
      componentResourceManager.ApplyResources((object) this.rbReportsSpeciesCN, "rbReportsSpeciesCN");
      this.rbReportsSpeciesCN.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbReportsSpeciesCN.ToggleGroupName = "tgSpecies";
      this.rbReportsSpeciesCN.PressedChanged += new EventHandler(this.rbReportsSpecies_PressedChanged);
      this.rbReportsSpeciesCN.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgSpecies_MeasureItem);
      this.rbReportsSpeciesCN.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgSpecies_DrawItem);
      this.rbReportsSpeciesSN.CanDepress = false;
      this.rbReportsSpeciesSN.Name = "rbReportsSpeciesSN";
      componentResourceManager.ApplyResources((object) this.rbReportsSpeciesSN, "rbReportsSpeciesSN");
      this.rbReportsSpeciesSN.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbReportsSpeciesSN.ToggleGroupName = "tgSpecies";
      this.rbReportsSpeciesSN.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgSpecies_MeasureItem);
      this.rbReportsSpeciesSN.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgSpecies_DrawItem);
      this.ribbonSeparator5.Name = "ribbonSeparator5";
      this.rbCheckBoxGPS.Enabled = false;
      this.rbCheckBoxGPS.Name = "rbCheckBoxGPS";
      componentResourceManager.ApplyResources((object) this.rbCheckBoxGPS, "rbCheckBoxGPS");
      this.rbCheckBoxGPS.CheckedChanged += new EventHandler(this.rbCheckBoxGPS_CheckedChanged);
      this.rbCheckBoxComments.Enabled = false;
      this.rbCheckBoxComments.Name = "rbCheckBoxComments";
      componentResourceManager.ApplyResources((object) this.rbCheckBoxComments, "rbCheckBoxComments");
      this.rbCheckBoxComments.CheckedChanged += new EventHandler(this.rbCheckBoxComments_CheckedChanged);
      this.rbCheckBoxUID.Enabled = false;
      this.rbCheckBoxUID.Name = "rbCheckBoxUID";
      componentResourceManager.ApplyResources((object) this.rbCheckBoxUID, "rbCheckBoxUID");
      this.rbCheckBoxUID.CheckedChanged += new EventHandler(this.rbCheckBoxUID_CheckedChanged);
      this.rbCheckBoxHideZeros.Enabled = false;
      this.rbCheckBoxHideZeros.Name = "rbCheckBoxHideZeros";
      componentResourceManager.ApplyResources((object) this.rbCheckBoxHideZeros, "rbCheckBoxHideZeros");
      this.rbCheckBoxHideZeros.CheckedChanged += new EventHandler(this.rbCheckBoxHudeZeros_CheckedChanged);
      this.rgModelNotes.Items.Add((RibbonItem) this.rmbModelProcessingNotes);
      this.rgModelNotes.Name = "rgModelNotes";
      this.rmbModelProcessingNotes.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.chart_organisation32;
      this.rmbModelProcessingNotes.Name = "rmbModelProcessingNotes";
      componentResourceManager.ApplyResources((object) this.rmbModelProcessingNotes, "rmbModelProcessingNotes");
      this.rmbModelProcessingNotes.Click += new EventHandler(this.rmbModelProcessingNotes_Click);
      this.rgMapping.Items.Add((RibbonItem) this.rbMapData);
      this.rgMapping.Items.Add((RibbonItem) this.rbReportsExportCSV);
      this.rgMapping.Items.Add((RibbonItem) this.rbReportsExportKML);
      this.rgMapping.Name = "rgMapping";
      this.rbMapData.Enabled = false;
      this.rbMapData.LargeImage = (Image) componentResourceManager.GetObject("rbMapData.LargeImage");
      this.rbMapData.Name = "rbMapData";
      componentResourceManager.ApplyResources((object) this.rbMapData, "rbMapData");
      this.rbMapData.Click += new EventHandler(this.rbMapData_Click);
      this.rbReportsExportCSV.Enabled = false;
      this.rbReportsExportCSV.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.csv_Large;
      this.rbReportsExportCSV.Name = "rbReportsExportCSV";
      this.rbReportsExportCSV.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.csv_Small;
      componentResourceManager.ApplyResources((object) this.rbReportsExportCSV, "rbReportsExportCSV");
      this.rbReportsExportCSV.Click += new EventHandler(this.rbReportsExportCSV_Click);
      this.rbReportsExportKML.Enabled = false;
      this.rbReportsExportKML.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.kml_Large;
      this.rbReportsExportKML.Name = "rbReportsExportKML";
      this.rbReportsExportKML.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.kml_Small;
      componentResourceManager.ApplyResources((object) this.rbReportsExportKML, "rbReportsExportKML");
      this.rbReportsExportKML.Click += new EventHandler(this.rbReportsExportKML_Click);
      this.rgExport.Items.Add((RibbonItem) this.rbReportExport);
      this.rgExport.Name = "rgExport";
      this.rbReportExport.LargeImage = (Image) componentResourceManager.GetObject("rbReportExport.LargeImage");
      this.rbReportExport.Name = "rbReportExport";
      componentResourceManager.ApplyResources((object) this.rbReportExport, "rbReportExport");
      this.rbReportExport.Click += new EventHandler(this.rbReportExport_Click);
      this.rtForecast.Enabled = false;
      this.rtForecast.Groups.Add(this.ForecastGroup);
      this.rtForecast.Groups.Add(this.BasicOptionsGroup);
      this.rtForecast.Groups.Add(this.CustomOptionsGroup);
      this.rtForecast.Groups.Add(this.ConfigurationsGroup);
      this.rtForecast.Groups.Add(this.rgForecastReports);
      this.rtForecast.Groups.Add(this.rgForecastUnits);
      this.rtForecast.Groups.Add(this.ForecastLockGroup);
      this.rtForecast.Name = "rtForecast";
      componentResourceManager.ApplyResources((object) this.rtForecast, "rtForecast");
      this.ForecastGroup.Items.Add((RibbonItem) this.ParametersReportButton);
      this.ForecastGroup.Items.Add((RibbonItem) this.RunForecastButton);
      this.ForecastGroup.Name = "ForecastGroup";
      this.ParametersReportButton.LargeImage = (Image) componentResourceManager.GetObject("ParametersReportButton.LargeImage");
      this.ParametersReportButton.Name = "ParametersReportButton";
      this.ParametersReportButton.SmallImage = (Image) componentResourceManager.GetObject("ParametersReportButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.ParametersReportButton, "ParametersReportButton");
      this.ParametersReportButton.Click += new EventHandler(this.ParametersReportButton_Click);
      this.RunForecastButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Run_Models;
      this.RunForecastButton.Name = "RunForecastButton";
      componentResourceManager.ApplyResources((object) this.RunForecastButton, "RunForecastButton");
      this.RunForecastButton.Click += new EventHandler(this.RunForecastButton_Click);
      this.BasicOptionsGroup.Items.Add((RibbonItem) this.ribbonToolBar1);
      this.BasicOptionsGroup.Items.Add((RibbonItem) this.ribbonToolBar2);
      this.BasicOptionsGroup.Items.Add((RibbonItem) this.BasicInputsButton);
      this.BasicOptionsGroup.Name = "BasicOptionsGroup";
      this.ribbonToolBar1.Name = "ribbonToolBar1";
      this.ribbonToolBar2.Name = "ribbonToolBar2";
      this.BasicInputsButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.basic_Large;
      this.BasicInputsButton.Name = "BasicInputsButton";
      this.BasicInputsButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.basic_Small;
      componentResourceManager.ApplyResources((object) this.BasicInputsButton, "BasicInputsButton");
      this.BasicInputsButton.Click += new EventHandler(this.BasicInputsButton_Click);
      this.CustomOptionsGroup.Items.Add((RibbonItem) this.MortalityButton);
      this.CustomOptionsGroup.Items.Add((RibbonItem) this.ReplantingButton);
      this.CustomOptionsGroup.Items.Add((RibbonItem) this.ExtremeEventsSplitButton);
      this.CustomOptionsGroup.Name = "CustomOptionsGroup";
      componentResourceManager.ApplyResources((object) this.CustomOptionsGroup, "CustomOptionsGroup");
      this.MortalityButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Reaper;
      this.MortalityButton.Name = "MortalityButton";
      componentResourceManager.ApplyResources((object) this.MortalityButton, "MortalityButton");
      this.MortalityButton.Click += new EventHandler(this.MortalityButton_Click);
      this.ReplantingButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Tree_Planting;
      this.ReplantingButton.Name = "ReplantingButton";
      componentResourceManager.ApplyResources((object) this.ReplantingButton, "ReplantingButton");
      this.ReplantingButton.Click += new EventHandler(this.ReplantingButton_Click);
      this.ExtremeEventsSplitButton.Items.Add((RibbonItem) this.PestsItem);
      this.ExtremeEventsSplitButton.Items.Add((RibbonItem) this.StormItem);
      this.ExtremeEventsSplitButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.extreme_events_Large;
      this.ExtremeEventsSplitButton.Name = "ExtremeEventsSplitButton";
      this.ExtremeEventsSplitButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.extreme_events_Small;
      componentResourceManager.ApplyResources((object) this.ExtremeEventsSplitButton, "ExtremeEventsSplitButton");
      this.PestsItem.Name = "PestsItem";
      componentResourceManager.ApplyResources((object) this.PestsItem, "PestsItem");
      this.PestsItem.Click += new EventHandler(this.PestsItem_Click);
      this.StormItem.Name = "StormItem";
      componentResourceManager.ApplyResources((object) this.StormItem, "StormItem");
      this.StormItem.Click += new EventHandler(this.StormItem_Click);
      this.ConfigurationsGroup.Items.Add((RibbonItem) this.ribbonLabel1);
      this.ConfigurationsGroup.Items.Add((RibbonItem) this.ConfigurationComboBox);
      this.ConfigurationsGroup.Items.Add((RibbonItem) this.ForecastRenameButton);
      this.ConfigurationsGroup.Items.Add((RibbonItem) this.NewForecastButton);
      this.ConfigurationsGroup.Items.Add((RibbonItem) this.CopyForecastButton);
      this.ConfigurationsGroup.Items.Add((RibbonItem) this.DeleteForecastButton);
      this.ConfigurationsGroup.Items.Add((RibbonItem) this.DefaultsButton);
      this.ConfigurationsGroup.Name = "ConfigurationsGroup";
      componentResourceManager.ApplyResources((object) this.ConfigurationsGroup, "ConfigurationsGroup");
      this.ribbonLabel1.Name = "ribbonLabel1";
      componentResourceManager.ApplyResources((object) this.ribbonLabel1, "ribbonLabel1");
      this.ConfigurationComboBox.DropDownStyle = RibbonComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.ConfigurationComboBox, "ConfigurationComboBox");
      this.ConfigurationComboBox.Name = "ConfigurationComboBox";
      this.ConfigurationComboBox.SelectedIndexChanged += new EventHandler(this.ConfigurationComboBox_SelectedIndexChanged);
      this.ForecastRenameButton.LargeImage = (Image) componentResourceManager.GetObject("ForecastRenameButton.LargeImage");
      this.ForecastRenameButton.Name = "ForecastRenameButton";
      this.ForecastRenameButton.SmallImage = (Image) componentResourceManager.GetObject("ForecastRenameButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.ForecastRenameButton, "ForecastRenameButton");
      this.ForecastRenameButton.Click += new EventHandler(this.ForecastRenameButton_Click);
      this.NewForecastButton.Name = "NewForecastButton";
      this.NewForecastButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.new_Small;
      componentResourceManager.ApplyResources((object) this.NewForecastButton, "NewForecastButton");
      this.NewForecastButton.Click += new EventHandler(this.NewForecastButton_Click);
      this.CopyForecastButton.Name = "CopyForecastButton";
      this.CopyForecastButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.copy_Small;
      componentResourceManager.ApplyResources((object) this.CopyForecastButton, "CopyForecastButton");
      this.CopyForecastButton.Click += new EventHandler(this.CopyForecastButton_Click);
      this.DeleteForecastButton.Name = "DeleteForecastButton";
      this.DeleteForecastButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.delete_Small;
      componentResourceManager.ApplyResources((object) this.DeleteForecastButton, "DeleteForecastButton");
      this.DeleteForecastButton.Click += new EventHandler(this.DeleteForecastButton_Click);
      this.DefaultsButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.reset_Large;
      this.DefaultsButton.Name = "DefaultsButton";
      this.DefaultsButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.reset_Small;
      componentResourceManager.ApplyResources((object) this.DefaultsButton, "DefaultsButton");
      this.DefaultsButton.Click += new EventHandler(this.DefaultsButton_Click);
      this.rgForecastReports.Enabled = false;
      this.rgForecastReports.Items.Add((RibbonItem) this.StructuralSplitButton);
      this.rgForecastReports.Items.Add((RibbonItem) this.FunctionalSplitButton);
      this.rgForecastReports.Name = "rgForecastReports";
      componentResourceManager.ApplyResources((object) this.rgForecastReports, "rgForecastReports");
      this.StructuralSplitButton.Items.Add((RibbonItem) this.ForecastPopulationSummaryLabel);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.NumTreesTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.NumTreesByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.PctCoverTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.PctCoverByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.TreeCoverAreaTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.TreeCoverAreaByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.DbhGrowthTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.DbhGrowthByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.DbhDistribTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.BasalAreaTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.BasalAreaByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.ForecastLeafAreaAndBiomassLabel);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.LeafAreaTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.LeafAreaByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.LeafAreaIndexTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.LeafAreaIndexByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.TotalLfBiomassTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.TotalLfBiomassByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.AreaLfBiomassTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.AreaLfBiomassByStrata);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.TreeBiomassTotal);
      this.StructuralSplitButton.Items.Add((RibbonItem) this.TreeBiomassByStrata);
      this.StructuralSplitButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.structural_Large;
      this.StructuralSplitButton.Name = "StructuralSplitButton";
      this.StructuralSplitButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.structural_Small;
      componentResourceManager.ApplyResources((object) this.StructuralSplitButton, "StructuralSplitButton");
      this.ForecastPopulationSummaryLabel.Name = "ForecastPopulationSummaryLabel";
      componentResourceManager.ApplyResources((object) this.ForecastPopulationSummaryLabel, "ForecastPopulationSummaryLabel");
      this.NumTreesTotal.Name = "NumTreesTotal";
      componentResourceManager.ApplyResources((object) this.NumTreesTotal, "NumTreesTotal");
      this.NumTreesTotal.Click += new EventHandler(this.NumTreesTotal_Click);
      this.NumTreesByStrata.Name = "NumTreesByStrata";
      componentResourceManager.ApplyResources((object) this.NumTreesByStrata, "NumTreesByStrata");
      this.NumTreesByStrata.Click += new EventHandler(this.NumTreesByStrata_Click);
      this.PctCoverTotal.Name = "PctCoverTotal";
      componentResourceManager.ApplyResources((object) this.PctCoverTotal, "PctCoverTotal");
      this.PctCoverTotal.Click += new EventHandler(this.PctCoverTotal_Click);
      this.PctCoverByStrata.Name = "PctCoverByStrata";
      componentResourceManager.ApplyResources((object) this.PctCoverByStrata, "PctCoverByStrata");
      this.PctCoverByStrata.Click += new EventHandler(this.PctCoverByStrata_Click);
      this.TreeCoverAreaTotal.Name = "TreeCoverAreaTotal";
      componentResourceManager.ApplyResources((object) this.TreeCoverAreaTotal, "TreeCoverAreaTotal");
      this.TreeCoverAreaTotal.Click += new EventHandler(this.TreeCoverAreaTotal_Click);
      this.TreeCoverAreaByStrata.Name = "TreeCoverAreaByStrata";
      componentResourceManager.ApplyResources((object) this.TreeCoverAreaByStrata, "TreeCoverAreaByStrata");
      this.TreeCoverAreaByStrata.Click += new EventHandler(this.TreeCoverAreaByStrata_Click);
      this.DbhGrowthTotal.Name = "DbhGrowthTotal";
      componentResourceManager.ApplyResources((object) this.DbhGrowthTotal, "DbhGrowthTotal");
      this.DbhGrowthTotal.Click += new EventHandler(this.DbhGrowthTotal_Click);
      this.DbhGrowthByStrata.Name = "DbhGrowthByStrata";
      componentResourceManager.ApplyResources((object) this.DbhGrowthByStrata, "DbhGrowthByStrata");
      this.DbhGrowthByStrata.Click += new EventHandler(this.DbhGrowthByStrata_Click);
      this.DbhDistribTotal.Name = "DbhDistribTotal";
      componentResourceManager.ApplyResources((object) this.DbhDistribTotal, "DbhDistribTotal");
      this.DbhDistribTotal.Click += new EventHandler(this.DbhDistribTotal_Click);
      this.BasalAreaTotal.Name = "BasalAreaTotal";
      componentResourceManager.ApplyResources((object) this.BasalAreaTotal, "BasalAreaTotal");
      this.BasalAreaTotal.Click += new EventHandler(this.BasalAreaTotal_Click);
      this.BasalAreaByStrata.Name = "BasalAreaByStrata";
      componentResourceManager.ApplyResources((object) this.BasalAreaByStrata, "BasalAreaByStrata");
      this.BasalAreaByStrata.Click += new EventHandler(this.BasalAreaByStrata_Click);
      this.ForecastLeafAreaAndBiomassLabel.Name = "ForecastLeafAreaAndBiomassLabel";
      componentResourceManager.ApplyResources((object) this.ForecastLeafAreaAndBiomassLabel, "ForecastLeafAreaAndBiomassLabel");
      this.LeafAreaTotal.Name = "LeafAreaTotal";
      componentResourceManager.ApplyResources((object) this.LeafAreaTotal, "LeafAreaTotal");
      this.LeafAreaTotal.Click += new EventHandler(this.LeafAreaTotal_Click);
      this.LeafAreaByStrata.Name = "LeafAreaByStrata";
      componentResourceManager.ApplyResources((object) this.LeafAreaByStrata, "LeafAreaByStrata");
      this.LeafAreaByStrata.Click += new EventHandler(this.LeafAreaByStrata_Click);
      this.LeafAreaIndexTotal.Name = "LeafAreaIndexTotal";
      componentResourceManager.ApplyResources((object) this.LeafAreaIndexTotal, "LeafAreaIndexTotal");
      this.LeafAreaIndexTotal.Click += new EventHandler(this.LeafAreaIndexTotal_Click);
      this.LeafAreaIndexByStrata.Name = "LeafAreaIndexByStrata";
      componentResourceManager.ApplyResources((object) this.LeafAreaIndexByStrata, "LeafAreaIndexByStrata");
      this.LeafAreaIndexByStrata.Click += new EventHandler(this.LeafAreaIndexByStrata_Click);
      this.TotalLfBiomassTotal.Name = "TotalLfBiomassTotal";
      componentResourceManager.ApplyResources((object) this.TotalLfBiomassTotal, "TotalLfBiomassTotal");
      this.TotalLfBiomassTotal.Click += new EventHandler(this.TotalLfBiomassTotal_Click);
      this.TotalLfBiomassByStrata.Name = "TotalLfBiomassByStrata";
      componentResourceManager.ApplyResources((object) this.TotalLfBiomassByStrata, "TotalLfBiomassByStrata");
      this.TotalLfBiomassByStrata.Click += new EventHandler(this.TotalLfBiomassByStrata_Click);
      this.AreaLfBiomassTotal.Name = "AreaLfBiomassTotal";
      componentResourceManager.ApplyResources((object) this.AreaLfBiomassTotal, "AreaLfBiomassTotal");
      this.AreaLfBiomassTotal.Click += new EventHandler(this.AreaLfBiomassTotal_Click);
      this.AreaLfBiomassByStrata.Name = "AreaLfBiomassByStrata";
      componentResourceManager.ApplyResources((object) this.AreaLfBiomassByStrata, "AreaLfBiomassByStrata");
      this.AreaLfBiomassByStrata.Click += new EventHandler(this.AreaLfBiomassByStrata_Click);
      this.TreeBiomassTotal.Name = "TreeBiomassTotal";
      componentResourceManager.ApplyResources((object) this.TreeBiomassTotal, "TreeBiomassTotal");
      this.TreeBiomassTotal.Click += new EventHandler(this.TreeBiomassTotal_Click);
      this.TreeBiomassByStrata.Name = "TreeBiomassByStrata";
      componentResourceManager.ApplyResources((object) this.TreeBiomassByStrata, "TreeBiomassByStrata");
      this.TreeBiomassByStrata.Click += new EventHandler(this.TreeBiomassByStrata_Click);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastCarbonLabel);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastCarbonStorage);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastCarbonStorageByStrata);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastCarbonSequestration);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastCarbonSequestrationByStrata);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantLabel);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantSummary);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantCO);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantNO2);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantO3);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantSO2);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantPM25);
      this.FunctionalSplitButton.Items.Add((RibbonItem) this.ForecastPollutantPM10);
      this.FunctionalSplitButton.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.written_report_Large;
      this.FunctionalSplitButton.Name = "FunctionalSplitButton";
      this.FunctionalSplitButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.written_report_Small;
      componentResourceManager.ApplyResources((object) this.FunctionalSplitButton, "FunctionalSplitButton");
      this.ForecastCarbonLabel.Name = "ForecastCarbonLabel";
      componentResourceManager.ApplyResources((object) this.ForecastCarbonLabel, "ForecastCarbonLabel");
      this.ForecastCarbonStorage.Name = "ForecastCarbonStorage";
      componentResourceManager.ApplyResources((object) this.ForecastCarbonStorage, "ForecastCarbonStorage");
      this.ForecastCarbonStorage.Click += new EventHandler(this.ForecastCarbonStorage_Click);
      this.ForecastCarbonStorageByStrata.Name = "ForecastCarbonStorageByStrata";
      componentResourceManager.ApplyResources((object) this.ForecastCarbonStorageByStrata, "ForecastCarbonStorageByStrata");
      this.ForecastCarbonStorageByStrata.Click += new EventHandler(this.ForecastCarbonStorageByStrata_Click);
      this.ForecastCarbonSequestration.Name = "ForecastCarbonSequestration";
      componentResourceManager.ApplyResources((object) this.ForecastCarbonSequestration, "ForecastCarbonSequestration");
      this.ForecastCarbonSequestration.Click += new EventHandler(this.ForecastCarbonSequestration_Click);
      this.ForecastCarbonSequestrationByStrata.Name = "ForecastCarbonSequestrationByStrata";
      componentResourceManager.ApplyResources((object) this.ForecastCarbonSequestrationByStrata, "ForecastCarbonSequestrationByStrata");
      this.ForecastCarbonSequestrationByStrata.Click += new EventHandler(this.ForecastCarbonSequestrationByStrata_Click);
      this.ForecastPollutantLabel.Name = "ForecastPollutantLabel";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantLabel, "ForecastPollutantLabel");
      this.ForecastPollutantSummary.Name = "ForecastPollutantSummary";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantSummary, "ForecastPollutantSummary");
      this.ForecastPollutantSummary.Click += new EventHandler(this.AllTotal_Click);
      this.ForecastPollutantCO.Name = "ForecastPollutantCO";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantCO, "ForecastPollutantCO");
      this.ForecastPollutantCO.Click += new EventHandler(this.COTotal_Click);
      this.ForecastPollutantNO2.Name = "ForecastPollutantNO2";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantNO2, "ForecastPollutantNO2");
      this.ForecastPollutantNO2.Click += new EventHandler(this.NO2Total_Click);
      this.ForecastPollutantO3.Name = "ForecastPollutantO3";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantO3, "ForecastPollutantO3");
      this.ForecastPollutantO3.Click += new EventHandler(this.O3Total_Click);
      this.ForecastPollutantSO2.Name = "ForecastPollutantSO2";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantSO2, "ForecastPollutantSO2");
      this.ForecastPollutantSO2.Click += new EventHandler(this.SO2Total_Click);
      this.ForecastPollutantPM25.Name = "ForecastPollutantPM25";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantPM25, "ForecastPollutantPM25");
      this.ForecastPollutantPM25.Click += new EventHandler(this.PM25Total_Click);
      this.ForecastPollutantPM10.Name = "ForecastPollutantPM10";
      componentResourceManager.ApplyResources((object) this.ForecastPollutantPM10, "ForecastPollutantPM10");
      this.ForecastPollutantPM10.Click += new EventHandler(this.PM10Total_Click);
      this.rgForecastUnits.Items.Add((RibbonItem) this.rtbForecastEnglish);
      this.rgForecastUnits.Items.Add((RibbonItem) this.rtbForecastMetric);
      this.rgForecastUnits.Name = "rgForecastUnits";
      componentResourceManager.ApplyResources((object) this.rgForecastUnits, "rgForecastUnits");
      this.rtbForecastEnglish.Name = "rtbForecastEnglish";
      this.rtbForecastEnglish.Pressed = true;
      componentResourceManager.ApplyResources((object) this.rtbForecastEnglish, "rtbForecastEnglish");
      this.rtbForecastEnglish.ToggleGroupName = "rgForecastUnits";
      this.rtbForecastEnglish.PressedChanged += new EventHandler(this.rbForecastReportUnits_PressedChanged);
      this.rtbForecastEnglish.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgUnits_MeasureItem);
      this.rtbForecastEnglish.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgUnits_DrawItem);
      this.rtbForecastMetric.Name = "rtbForecastMetric";
      componentResourceManager.ApplyResources((object) this.rtbForecastMetric, "rtbForecastMetric");
      this.rtbForecastMetric.ToggleGroupName = "rgForecastUnits";
      this.rtbForecastMetric.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgUnits_MeasureItem);
      this.rtbForecastMetric.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgUnits_DrawItem);
      this.ForecastLockGroup.Items.Add((RibbonItem) this.ForecastEditDataButton);
      this.ForecastLockGroup.Name = "ForecastLockGroup";
      this.ForecastEditDataButton.LargeImage = (Image) componentResourceManager.GetObject("ForecastEditDataButton.LargeImage");
      this.ForecastEditDataButton.Name = "ForecastEditDataButton";
      componentResourceManager.ApplyResources((object) this.ForecastEditDataButton, "ForecastEditDataButton");
      this.ForecastEditDataButton.Click += new EventHandler(this.ForecastEditDataButton_Click);
      this.rtData.Enabled = false;
      this.rtData.Groups.Add(this.rgDataCollection);
      this.rtData.Groups.Add(this.rgInventoryData);
      this.rtData.Groups.Add(this.rgInventoryValue);
      this.rtData.Groups.Add(this.rgReportClasses);
      this.rtData.Groups.Add(this.rgDataExport);
      this.rtData.Groups.Add(this.rgDataActions);
      this.rtData.Groups.Add(this.rgDataMode);
      this.rtData.Name = "rtData";
      componentResourceManager.ApplyResources((object) this.rtData, "rtData");
      this.rgDataCollection.Items.Add((RibbonItem) this.rbSubmitMobileData);
      this.rgDataCollection.Items.Add((RibbonItem) this.rbRetrieveMobileData);
      this.rgDataCollection.Items.Add((RibbonItem) ribbonSeparator);
      this.rgDataCollection.Items.Add((RibbonItem) this.rbtnPaperForm1);
      this.rgDataCollection.Items.Add((RibbonItem) this.rsImport);
      this.rgDataCollection.Items.Add((RibbonItem) this.rbImport);
      this.rgDataCollection.Name = "rgDataCollection";
      componentResourceManager.ApplyResources((object) this.rgDataCollection, "rgDataCollection");
      this.rbSubmitMobileData.LargeImage = (Image) componentResourceManager.GetObject("rbSubmitMobileData.LargeImage");
      this.rbSubmitMobileData.Name = "rbSubmitMobileData";
      componentResourceManager.ApplyResources((object) this.rbSubmitMobileData, "rbSubmitMobileData");
      this.rbSubmitMobileData.Click += new EventHandler(this.rbSubmitMobileData_Click);
      this.rbRetrieveMobileData.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.retrieve_data_Large;
      this.rbRetrieveMobileData.Name = "rbRetrieveMobileData";
      componentResourceManager.ApplyResources((object) this.rbRetrieveMobileData, "rbRetrieveMobileData");
      this.rbRetrieveMobileData.Click += new EventHandler(this.rbRetrieveMobileData_Click);
      this.rbtnPaperForm1.LargeImage = (Image) componentResourceManager.GetObject("rbtnPaperForm1.LargeImage");
      this.rbtnPaperForm1.Name = "rbtnPaperForm1";
      componentResourceManager.ApplyResources((object) this.rbtnPaperForm1, "rbtnPaperForm1");
      this.rbtnPaperForm1.Click += new EventHandler(this.rbtnPaperForm1_Click);
      this.rsImport.Name = "rsImport";
      this.rbImport.LargeImage = (Image) componentResourceManager.GetObject("rbImport.LargeImage");
      this.rbImport.Name = "rbImport";
      componentResourceManager.ApplyResources((object) this.rbImport, "rbImport");
      this.rbImport.Click += new EventHandler(this.rbImport_Click);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbDataPlots);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbDataReferenceObjects);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbDataGroundCovers);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbDataLandUses);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbDataTrees);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbDataShrubs);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbDataPlantingSites);
      this.rgInventoryData.Items.Add((RibbonItem) this.rbValidate);
      this.rgInventoryData.Name = "rgInventoryData";
      componentResourceManager.ApplyResources((object) this.rgInventoryData, "rgInventoryData");
      this.rbDataPlots.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.plots_Large;
      this.rbDataPlots.Name = "rbDataPlots";
      this.rbDataPlots.SelectableInListItem = true;
      componentResourceManager.ApplyResources((object) this.rbDataPlots, "rbDataPlots");
      this.rbDataPlots.Click += new EventHandler(this.rbtnDataPlots_Click);
      this.rbDataReferenceObjects.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.reference_objects_Large;
      this.rbDataReferenceObjects.Name = "rbDataReferenceObjects";
      componentResourceManager.ApplyResources((object) this.rbDataReferenceObjects, "rbDataReferenceObjects");
      this.rbDataReferenceObjects.Click += new EventHandler(this.rbDataReferenceObjects_Click);
      this.rbDataGroundCovers.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.ground_cover_Large;
      this.rbDataGroundCovers.Name = "rbDataGroundCovers";
      componentResourceManager.ApplyResources((object) this.rbDataGroundCovers, "rbDataGroundCovers");
      this.rbDataGroundCovers.Click += new EventHandler(this.rbDataGroundCovers_Click);
      this.rbDataLandUses.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.land_use_Large;
      this.rbDataLandUses.Name = "rbDataLandUses";
      componentResourceManager.ApplyResources((object) this.rbDataLandUses, "rbDataLandUses");
      this.rbDataLandUses.Click += new EventHandler(this.rbDataLandUses_Click);
      this.rbDataTrees.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.trees_Large;
      this.rbDataTrees.Name = "rbDataTrees";
      componentResourceManager.ApplyResources((object) this.rbDataTrees, "rbDataTrees");
      this.rbDataTrees.Click += new EventHandler(this.rbDataTrees_Click);
      this.rbDataShrubs.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Shrubs;
      this.rbDataShrubs.Name = "rbDataShrubs";
      componentResourceManager.ApplyResources((object) this.rbDataShrubs, "rbDataShrubs");
      this.rbDataShrubs.Click += new EventHandler(this.rbDataShrubs_Click);
      this.rbDataPlantingSites.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.planting_sites_Large;
      this.rbDataPlantingSites.Name = "rbDataPlantingSites";
      componentResourceManager.ApplyResources((object) this.rbDataPlantingSites, "rbDataPlantingSites");
      this.rbDataPlantingSites.Click += new EventHandler(this.rbDataPlantingSites_Click);
      this.rbValidate.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.checkbox32;
      this.rbValidate.Name = "rbValidate";
      componentResourceManager.ApplyResources((object) this.rbValidate, "rbValidate");
      this.rbValidate.Click += new EventHandler(this.rbValidate_Click);
      this.rgInventoryValue.Items.Add((RibbonItem) this.rbtnBenefitPrices);
      this.rgInventoryValue.Items.Add((RibbonItem) this.rbAnnualCosts);
      this.rgInventoryValue.Name = "rgInventoryValue";
      componentResourceManager.ApplyResources((object) this.rgInventoryValue, "rgInventoryValue");
      this.rbtnBenefitPrices.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.benefit_prices_Large;
      this.rbtnBenefitPrices.Name = "rbtnBenefitPrices";
      componentResourceManager.ApplyResources((object) this.rbtnBenefitPrices, "rbtnBenefitPrices");
      this.rbtnBenefitPrices.Click += new EventHandler(this.rbtnBenefitPrices_Click);
      this.rbAnnualCosts.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.money_dollar32;
      this.rbAnnualCosts.Name = "rbAnnualCosts";
      componentResourceManager.ApplyResources((object) this.rbAnnualCosts, "rbAnnualCosts");
      this.rbAnnualCosts.Click += new EventHandler(this.rbAnnualCosts_Click);
      this.rgReportClasses.Items.Add((RibbonItem) this.rbRptDBH);
      this.rgReportClasses.Items.Add((RibbonItem) this.rbHealthClasses);
      this.rgReportClasses.Name = "rgReportClasses";
      componentResourceManager.ApplyResources((object) this.rgReportClasses, "rgReportClasses");
      this.rbRptDBH.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.dbh_classes;
      this.rbRptDBH.Name = "rbRptDBH";
      componentResourceManager.ApplyResources((object) this.rbRptDBH, "rbRptDBH");
      this.rbRptDBH.Click += new EventHandler(this.rbDBHClasses_Click);
      componentResourceManager.ApplyResources((object) this.rbHealthClasses, "rbHealthClasses");
      this.rbHealthClasses.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.trees_stetho32;
      this.rbHealthClasses.Name = "rbHealthClasses";
      this.rbHealthClasses.Click += new EventHandler(this.rbHealthClasses_Click);
      this.rgDataExport.Items.Add((RibbonItem) this.rbDataExportCSV);
      this.rgDataExport.Items.Add((RibbonItem) this.rbDataExportKML);
      this.rgDataExport.Name = "rgDataExport";
      componentResourceManager.ApplyResources((object) this.rgDataExport, "rgDataExport");
      this.rbDataExportCSV.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.csv_Large;
      this.rbDataExportCSV.Name = "rbDataExportCSV";
      this.rbDataExportCSV.SmallImage = (Image) componentResourceManager.GetObject("rbDataExportCSV.SmallImage");
      componentResourceManager.ApplyResources((object) this.rbDataExportCSV, "rbDataExportCSV");
      this.rbDataExportCSV.Click += new EventHandler(this.rbExportCSV_Click);
      this.rbDataExportKML.LargeImage = (Image) componentResourceManager.GetObject("rbDataExportKML.LargeImage");
      this.rbDataExportKML.Name = "rbDataExportKML";
      this.rbDataExportKML.SmallImage = (Image) componentResourceManager.GetObject("rbDataExportKML.SmallImage");
      componentResourceManager.ApplyResources((object) this.rbDataExportKML, "rbDataExportKML");
      this.rbDataExportKML.Click += new EventHandler(this.rbDataExportKML_Click);
      this.rgDataActions.Items.Add((RibbonItem) this.rbDataNew);
      this.rgDataActions.Items.Add((RibbonItem) this.rbDataCopy);
      this.rgDataActions.Items.Add((RibbonItem) this.rbDataUndo);
      this.rgDataActions.Items.Add((RibbonItem) this.rbDataRedo);
      this.rgDataActions.Items.Add((RibbonItem) this.rbDataDefaults);
      this.rgDataActions.Items.Add((RibbonItem) this.rbDataDelete);
      this.rgDataActions.Name = "rgDataActions";
      componentResourceManager.ApplyResources((object) this.rgDataActions, "rgDataActions");
      this.rgDataActions.DialogLauncherClick += new EventHandler(this.rgActions_DialogLauncherClick);
      this.rbDataNew.LargeImage = (Image) componentResourceManager.GetObject("rbDataNew.LargeImage");
      this.rbDataNew.Name = "rbDataNew";
      componentResourceManager.ApplyResources((object) this.rbDataNew, "rbDataNew");
      this.rbDataNew.Click += new EventHandler(this.rbDataNew_Click);
      this.rbDataCopy.LargeImage = (Image) componentResourceManager.GetObject("rbDataCopy.LargeImage");
      this.rbDataCopy.Name = "rbDataCopy";
      componentResourceManager.ApplyResources((object) this.rbDataCopy, "rbDataCopy");
      this.rbDataCopy.Click += new EventHandler(this.rbDataCopy_Click);
      this.rbDataUndo.LargeImage = (Image) componentResourceManager.GetObject("rbDataUndo.LargeImage");
      this.rbDataUndo.Name = "rbDataUndo";
      componentResourceManager.ApplyResources((object) this.rbDataUndo, "rbDataUndo");
      this.rbDataUndo.Click += new EventHandler(this.rbDataUndo_Click);
      this.rbDataRedo.LargeImage = (Image) componentResourceManager.GetObject("rbDataRedo.LargeImage");
      this.rbDataRedo.Name = "rbDataRedo";
      componentResourceManager.ApplyResources((object) this.rbDataRedo, "rbDataRedo");
      this.rbDataRedo.Click += new EventHandler(this.rbDataRedo_Click);
      this.rbDataDefaults.LargeImage = (Image) componentResourceManager.GetObject("rbDataDefaults.LargeImage");
      this.rbDataDefaults.Name = "rbDataDefaults";
      this.rbDataDefaults.SmallImage = (Image) componentResourceManager.GetObject("rbDataDefaults.SmallImage");
      componentResourceManager.ApplyResources((object) this.rbDataDefaults, "rbDataDefaults");
      this.rbDataDefaults.Click += new EventHandler(this.rbDataDefaults_Click);
      this.rbDataDelete.LargeImage = (Image) componentResourceManager.GetObject("rbDataDelete.LargeImage");
      this.rbDataDelete.Name = "rbDataDelete";
      componentResourceManager.ApplyResources((object) this.rbDataDelete, "rbDataDelete");
      this.rbDataDelete.Click += new EventHandler(this.rbDataDelete_Click);
      this.rgDataMode.Items.Add((RibbonItem) this.rbDataEnableEdit);
      this.rgDataMode.Name = "rgDataMode";
      this.rbDataEnableEdit.LargeImage = (Image) componentResourceManager.GetObject("rbDataEnableEdit.LargeImage");
      this.rbDataEnableEdit.Name = "rbDataEnableEdit";
      componentResourceManager.ApplyResources((object) this.rbDataEnableEdit, "rbDataEnableEdit");
      this.rbDataEnableEdit.Click += new EventHandler(this.rbDataEnableEdit_Click);
      this.rgPlot.Items.Add((RibbonItem) this.rbtnPlotRefObjects);
      this.rgPlot.Items.Add((RibbonItem) this.rbtnPlotLandUses);
      this.rgPlot.Items.Add((RibbonItem) this.rbtnPlotGroundCovers);
      this.rgPlot.Items.Add((RibbonItem) this.rbtnPlotTrees);
      this.rgPlot.Items.Add((RibbonItem) this.rbtnPlotShrubs);
      this.rgPlot.Items.Add((RibbonItem) this.rbtnPlotPlantingSites);
      this.rgPlot.Name = "rgPlot";
      componentResourceManager.ApplyResources((object) this.rgPlot, "rgPlot");
      this.rbtnPlotRefObjects.LargeImage = (Image) componentResourceManager.GetObject("rbtnPlotRefObjects.LargeImage");
      this.rbtnPlotRefObjects.Name = "rbtnPlotRefObjects";
      componentResourceManager.ApplyResources((object) this.rbtnPlotRefObjects, "rbtnPlotRefObjects");
      this.rbtnPlotRefObjects.Click += new EventHandler(this.rbtnPlotRefObjects_Click);
      this.rbtnPlotLandUses.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.land_use_Large;
      this.rbtnPlotLandUses.Name = "rbtnPlotLandUses";
      componentResourceManager.ApplyResources((object) this.rbtnPlotLandUses, "rbtnPlotLandUses");
      this.rbtnPlotLandUses.Click += new EventHandler(this.rbtnPlotLandUses_Click);
      this.rbtnPlotGroundCovers.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.ground_cover_Large;
      this.rbtnPlotGroundCovers.Name = "rbtnPlotGroundCovers";
      componentResourceManager.ApplyResources((object) this.rbtnPlotGroundCovers, "rbtnPlotGroundCovers");
      this.rbtnPlotGroundCovers.Click += new EventHandler(this.rbtnPlotGroundCover_Click);
      this.rbtnPlotTrees.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.trees_Large;
      this.rbtnPlotTrees.Name = "rbtnPlotTrees";
      componentResourceManager.ApplyResources((object) this.rbtnPlotTrees, "rbtnPlotTrees");
      this.rbtnPlotTrees.Click += new EventHandler(this.rbtnPlotTrees_Click);
      this.rbtnPlotShrubs.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Shrubs;
      this.rbtnPlotShrubs.Name = "rbtnPlotShrubs";
      componentResourceManager.ApplyResources((object) this.rbtnPlotShrubs, "rbtnPlotShrubs");
      this.rbtnPlotShrubs.Click += new EventHandler(this.rbtnPlotShrubs_Click);
      this.rbtnPlotPlantingSites.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.planting_sites_Large;
      this.rbtnPlotPlantingSites.Name = "rbtnPlotPlantingSites";
      componentResourceManager.ApplyResources((object) this.rbtnPlotPlantingSites, "rbtnPlotPlantingSites");
      this.rbtnPlotPlantingSites.Click += new EventHandler(this.rbtnPlotPlantingSites_Click);
      this.rtProject.Enabled = false;
      this.rtProject.Groups.Add(this.rgProject);
      this.rtProject.Groups.Add(this.rgDataFields);
      this.rtProject.Groups.Add(this.rgDefinePlots);
      this.rtProject.Groups.Add(this.rgStrata);
      this.rtProject.Groups.Add(this.rgProjectExport);
      this.rtProject.Groups.Add(this.rgProjectActions);
      this.rtProject.Groups.Add(this.rgProjectEditData);
      this.rtProject.Name = "rtProject";
      componentResourceManager.ApplyResources((object) this.rtProject, "rtProject");
      this.rgProject.Items.Add((RibbonItem) this.rbtnOverview);
      this.rgProject.Name = "rgProject";
      this.rbtnOverview.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.input_Large;
      this.rbtnOverview.Name = "rbtnOverview";
      componentResourceManager.ApplyResources((object) this.rbtnOverview, "rbtnOverview");
      this.rbtnOverview.Click += new EventHandler(this.rbtnOverview_Click);
      this.rgDataFields.Items.Add((RibbonItem) this.rbtnLandUses);
      this.rgDataFields.Items.Add((RibbonItem) this.rbtnGroundCovers);
      this.rgDataFields.Items.Add((RibbonItem) this.rbDBH);
      this.rgDataFields.Items.Add((RibbonItem) this.rbCondition);
      this.rgDataFields.Items.Add((RibbonItem) this.rbDieback);
      this.rgDataFields.Items.Add((RibbonItem) this.rbtnStreets);
      this.rgDataFields.Items.Add((RibbonItem) this.rbtnLocSites);
      this.rgDataFields.Items.Add((RibbonItem) this.rbPlantingSiteTypes);
      this.rgDataFields.Items.Add((RibbonItem) this.rmMaintenance);
      this.rgDataFields.Items.Add((RibbonItem) this.rmOther);
      this.rgDataFields.Name = "rgDataFields";
      componentResourceManager.ApplyResources((object) this.rgDataFields, "rgDataFields");
      this.rbtnLandUses.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.land_use_Large;
      this.rbtnLandUses.Name = "rbtnLandUses";
      this.rbtnLandUses.SupportedGroupSizing = SupportedGroupSizing.TextAlwaysVisible;
      componentResourceManager.ApplyResources((object) this.rbtnLandUses, "rbtnLandUses");
      this.rbtnLandUses.Click += new EventHandler(this.rbtnLandUses_Click);
      this.rbtnGroundCovers.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.ground_cover_Large;
      this.rbtnGroundCovers.Name = "rbtnGroundCovers";
      componentResourceManager.ApplyResources((object) this.rbtnGroundCovers, "rbtnGroundCovers");
      this.rbtnGroundCovers.Click += new EventHandler(this.rbtnGroundCovers_Click);
      this.rbDBH.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.dbh_classes;
      this.rbDBH.Name = "rbDBH";
      componentResourceManager.ApplyResources((object) this.rbDBH, "rbDBH");
      this.rbDBH.Click += new EventHandler(this.rbDBH_Click);
      this.rbCondition.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.trees_stetho32;
      this.rbCondition.Name = "rbCondition";
      componentResourceManager.ApplyResources((object) this.rbCondition, "rbCondition");
      this.rbCondition.Click += new EventHandler(this.rbCondition_Click);
      this.rbDieback.LargeImage = (Image) componentResourceManager.GetObject("rbDieback.LargeImage");
      this.rbDieback.Name = "rbDieback";
      componentResourceManager.ApplyResources((object) this.rbDieback, "rbDieback");
      this.rbDieback.Click += new EventHandler(this.rbDieback_Click);
      this.rbtnStreets.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.street_names_Large;
      this.rbtnStreets.Name = "rbtnStreets";
      componentResourceManager.ApplyResources((object) this.rbtnStreets, "rbtnStreets");
      this.rbtnStreets.Click += new EventHandler(this.rbtnStreets_Click);
      this.rbtnLocSites.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.location_site_Large;
      this.rbtnLocSites.Name = "rbtnLocSites";
      componentResourceManager.ApplyResources((object) this.rbtnLocSites, "rbtnLocSites");
      this.rbtnLocSites.Click += new EventHandler(this.rbtnLocSites_Click);
      this.rbPlantingSiteTypes.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.planting_sites_Large;
      this.rbPlantingSiteTypes.Name = "rbPlantingSiteTypes";
      this.rbPlantingSiteTypes.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.planting_sites_Small;
      componentResourceManager.ApplyResources((object) this.rbPlantingSiteTypes, "rbPlantingSiteTypes");
      this.rbPlantingSiteTypes.Click += new EventHandler(this.rbPlantingSiteTypes_Click);
      this.rmMaintenance.Items.Add((RibbonItem) this.rbtnMaintenanceRecommended);
      this.rmMaintenance.Items.Add((RibbonItem) this.rbtnMaintenanceTasks);
      this.rmMaintenance.Items.Add((RibbonItem) this.rbtnSidewalkConflicts);
      this.rmMaintenance.Items.Add((RibbonItem) this.rbtnUtilityConflicts);
      this.rmMaintenance.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.Maintenance;
      this.rmMaintenance.Name = "rmMaintenance";
      componentResourceManager.ApplyResources((object) this.rmMaintenance, "rmMaintenance");
      this.rbtnMaintenanceRecommended.Name = "rbtnMaintenanceRecommended";
      componentResourceManager.ApplyResources((object) this.rbtnMaintenanceRecommended, "rbtnMaintenanceRecommended");
      this.rbtnMaintenanceRecommended.Click += new EventHandler(this.rbtnMaintRec_Click);
      this.rbtnMaintenanceTasks.Name = "rbtnMaintenanceTasks";
      componentResourceManager.ApplyResources((object) this.rbtnMaintenanceTasks, "rbtnMaintenanceTasks");
      this.rbtnMaintenanceTasks.Click += new EventHandler(this.rbtnMaintTasks_Click);
      this.rbtnSidewalkConflicts.Name = "rbtnSidewalkConflicts";
      componentResourceManager.ApplyResources((object) this.rbtnSidewalkConflicts, "rbtnSidewalkConflicts");
      this.rbtnSidewalkConflicts.Click += new EventHandler(this.rbSidewalkConflict_Click);
      this.rbtnUtilityConflicts.Name = "rbtnUtilityConflicts";
      componentResourceManager.ApplyResources((object) this.rbtnUtilityConflicts, "rbtnUtilityConflicts");
      this.rbtnUtilityConflicts.Click += new EventHandler(this.rbUtilityConflicts_Click);
      this.rmOther.Items.Add((RibbonItem) this.rbtnFieldOne);
      this.rmOther.Items.Add((RibbonItem) this.rbtnFieldTwo);
      this.rmOther.Items.Add((RibbonItem) this.rbtnFieldThree);
      this.rmOther.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.wrench32;
      this.rmOther.Name = "rmOther";
      componentResourceManager.ApplyResources((object) this.rmOther, "rmOther");
      this.rbtnFieldOne.Name = "rbtnFieldOne";
      componentResourceManager.ApplyResources((object) this.rbtnFieldOne, "rbtnFieldOne");
      this.rbtnFieldOne.Click += new EventHandler(this.rbtnOtherOne_Click);
      this.rbtnFieldTwo.Name = "rbtnFieldTwo";
      componentResourceManager.ApplyResources((object) this.rbtnFieldTwo, "rbtnFieldTwo");
      this.rbtnFieldTwo.Click += new EventHandler(this.rbtnOtherTwo_Click);
      this.rbtnFieldThree.Name = "rbtnFieldThree";
      componentResourceManager.ApplyResources((object) this.rbtnFieldThree, "rbtnFieldThree");
      this.rbtnFieldThree.Click += new EventHandler(this.rbtnOtherThree_Click);
      this.rgDefinePlots.Items.Add((RibbonItem) this.loadPlotsFromFileButton);
      this.rgDefinePlots.Items.Add((RibbonItem) this.definePlotsViaGoogleMaps);
      this.rgDefinePlots.Items.Add((RibbonItem) this.definePlotsManuallyButton);
      this.rgDefinePlots.Name = "rgDefinePlots";
      componentResourceManager.ApplyResources((object) this.rgDefinePlots, "rgDefinePlots");
      this.loadPlotsFromFileButton.Name = "loadPlotsFromFileButton";
      this.loadPlotsFromFileButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.load_file_folder_Small;
      componentResourceManager.ApplyResources((object) this.loadPlotsFromFileButton, "loadPlotsFromFileButton");
      this.loadPlotsFromFileButton.Click += new EventHandler(this.loadPlotsFromFileButton_Click);
      this.definePlotsViaGoogleMaps.Name = "definePlotsViaGoogleMaps";
      this.definePlotsViaGoogleMaps.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.via_google_Small;
      componentResourceManager.ApplyResources((object) this.definePlotsViaGoogleMaps, "definePlotsViaGoogleMaps");
      this.definePlotsViaGoogleMaps.Click += new EventHandler(this.definePlotsViaGoogleMaps_Click);
      this.definePlotsManuallyButton.Name = "definePlotsManuallyButton";
      this.definePlotsManuallyButton.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.create_manually_Small;
      componentResourceManager.ApplyResources((object) this.definePlotsManuallyButton, "definePlotsManuallyButton");
      this.definePlotsManuallyButton.Click += new EventHandler(this.definePlotsManuallyButton_Click);
      this.rgStrata.Items.Add((RibbonItem) this.rbStrata);
      this.rgStrata.Name = "rgStrata";
      this.rbStrata.LargeImage = (Image) componentResourceManager.GetObject("rbStrata.LargeImage");
      this.rbStrata.Name = "rbStrata";
      this.rbStrata.SupportedGroupSizing = SupportedGroupSizing.LargeImageOnly;
      componentResourceManager.ApplyResources((object) this.rbStrata, "rbStrata");
      this.rbStrata.Click += new EventHandler(this.rbStrata_Click);
      this.rgProjectExport.Items.Add((RibbonItem) this.rbProjectExportCSV);
      this.rgProjectExport.Name = "rgProjectExport";
      componentResourceManager.ApplyResources((object) this.rgProjectExport, "rgProjectExport");
      this.rbProjectExportCSV.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.csv_Large;
      this.rbProjectExportCSV.Name = "rbProjectExportCSV";
      this.rbProjectExportCSV.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.csv_Small;
      componentResourceManager.ApplyResources((object) this.rbProjectExportCSV, "rbProjectExportCSV");
      this.rbProjectExportCSV.Click += new EventHandler(this.rbExportCSV_Click);
      this.rgProjectActions.Enabled = false;
      this.rgProjectActions.Items.Add((RibbonItem) this.rbProjectNew);
      this.rgProjectActions.Items.Add((RibbonItem) this.rbProjectCopy);
      this.rgProjectActions.Items.Add((RibbonItem) this.rbProjectUndo);
      this.rgProjectActions.Items.Add((RibbonItem) this.rbProjectRedo);
      this.rgProjectActions.Items.Add((RibbonItem) this.rbProjectDefaults);
      this.rgProjectActions.Items.Add((RibbonItem) this.rbProjectDelete);
      this.rgProjectActions.Name = "rgProjectActions";
      componentResourceManager.ApplyResources((object) this.rgProjectActions, "rgProjectActions");
      this.rgProjectActions.DialogLauncherClick += new EventHandler(this.rgActions1_DialogLauncherClick);
      this.rbProjectNew.LargeImage = (Image) componentResourceManager.GetObject("rbProjectNew.LargeImage");
      this.rbProjectNew.Name = "rbProjectNew";
      componentResourceManager.ApplyResources((object) this.rbProjectNew, "rbProjectNew");
      this.rbProjectNew.Click += new EventHandler(this.rbProjectNew_Click);
      this.rbProjectCopy.LargeImage = (Image) componentResourceManager.GetObject("rbProjectCopy.LargeImage");
      this.rbProjectCopy.Name = "rbProjectCopy";
      componentResourceManager.ApplyResources((object) this.rbProjectCopy, "rbProjectCopy");
      this.rbProjectCopy.Click += new EventHandler(this.rbProjectCopy_Click);
      this.rbProjectUndo.LargeImage = (Image) componentResourceManager.GetObject("rbProjectUndo.LargeImage");
      this.rbProjectUndo.Name = "rbProjectUndo";
      componentResourceManager.ApplyResources((object) this.rbProjectUndo, "rbProjectUndo");
      this.rbProjectUndo.Click += new EventHandler(this.rbProjectUndo_Click);
      this.rbProjectRedo.LargeImage = (Image) componentResourceManager.GetObject("rbProjectRedo.LargeImage");
      this.rbProjectRedo.Name = "rbProjectRedo";
      componentResourceManager.ApplyResources((object) this.rbProjectRedo, "rbProjectRedo");
      this.rbProjectRedo.Click += new EventHandler(this.rbProjectRedo_Click);
      this.rbProjectDefaults.LargeImage = (Image) componentResourceManager.GetObject("rbProjectDefaults.LargeImage");
      this.rbProjectDefaults.Name = "rbProjectDefaults";
      this.rbProjectDefaults.SmallImage = (Image) componentResourceManager.GetObject("rbProjectDefaults.SmallImage");
      componentResourceManager.ApplyResources((object) this.rbProjectDefaults, "rbProjectDefaults");
      this.rbProjectDefaults.Click += new EventHandler(this.rbProjectDefaults_Click);
      this.rbProjectDelete.LargeImage = (Image) componentResourceManager.GetObject("rbProjectDelete.LargeImage");
      this.rbProjectDelete.Name = "rbProjectDelete";
      componentResourceManager.ApplyResources((object) this.rbProjectDelete, "rbProjectDelete");
      this.rbProjectDelete.Click += new EventHandler(this.rbProjectDelete_Click);
      this.rgProjectEditData.Items.Add((RibbonItem) this.rbProjectEnableEdit);
      this.rgProjectEditData.Name = "rgProjectEditData";
      this.rbProjectEnableEdit.LargeImage = (Image) componentResourceManager.GetObject("rbProjectEnableEdit.LargeImage");
      this.rbProjectEnableEdit.Name = "rbProjectEnableEdit";
      componentResourceManager.ApplyResources((object) this.rbProjectEnableEdit, "rbProjectEnableEdit");
      this.rbProjectEnableEdit.Click += new EventHandler(this.rbProjectEnableEdit_Click);
      this.configToolBar.Items.Add((RibbonItem) this.minimizeRibbonButton);
      this.configToolBar.Items.Add((RibbonItem) this.expandRibbonButton);
      this.configToolBar.Items.Add((RibbonItem) this.helpConfigButton);
      this.configToolBar.Name = "configToolBar";
      componentResourceManager.ApplyResources((object) this.minimizeRibbonButton, "minimizeRibbonButton");
      this.minimizeRibbonButton.Name = "minimizeRibbonButton";
      this.minimizeRibbonButton.SmallImage = (Image) componentResourceManager.GetObject("minimizeRibbonButton.SmallImage");
      this.minimizeRibbonButton.Visible = false;
      this.minimizeRibbonButton.Click += new EventHandler(this.minimizeRibbonButton_Click);
      componentResourceManager.ApplyResources((object) this.expandRibbonButton, "expandRibbonButton");
      this.expandRibbonButton.Name = "expandRibbonButton";
      this.expandRibbonButton.SmallImage = (Image) componentResourceManager.GetObject("expandRibbonButton.SmallImage");
      this.expandRibbonButton.Visible = false;
      this.expandRibbonButton.Click += new EventHandler(this.expandRibbonButton_Click);
      this.helpConfigButton.Name = "helpConfigButton";
      this.helpConfigButton.SmallImage = (Image) componentResourceManager.GetObject("helpConfigButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.helpConfigButton, "helpConfigButton");
      this.helpConfigButton.Visible = false;
      this.colorSchemeMenu.Items.Add((RibbonItem) this.themeColorMenu);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.themeLightnessMenu);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.ribbonSeparator7);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.customButton);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.blue2007Button);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.silver2007Button);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.black2007Button);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.blue2010Button);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.silver2010Button);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.black2010Button);
      this.colorSchemeMenu.Items.Add((RibbonItem) this.windows7Button);
      componentResourceManager.ApplyResources((object) this.colorSchemeMenu, "colorSchemeMenu");
      this.colorSchemeMenu.Name = "colorSchemeMenu";
      this.colorSchemeMenu.SmallImage = (Image) componentResourceManager.GetObject("colorSchemeMenu.SmallImage");
      this.themeColorMenu.Items.Add((RibbonItem) this.azureButton);
      this.themeColorMenu.Items.Add((RibbonItem) this.blueButton);
      this.themeColorMenu.Items.Add((RibbonItem) this.greenButton);
      this.themeColorMenu.Items.Add((RibbonItem) this.orangeButton);
      this.themeColorMenu.Items.Add((RibbonItem) this.orchidButton);
      this.themeColorMenu.Items.Add((RibbonItem) this.redButton);
      this.themeColorMenu.Items.Add((RibbonItem) this.tealButton);
      this.themeColorMenu.Items.Add((RibbonItem) this.violetButton);
      this.themeColorMenu.Name = "themeColorMenu";
      componentResourceManager.ApplyResources((object) this.themeColorMenu, "themeColorMenu");
      this.azureButton.CanDepress = false;
      this.azureButton.Name = "azureButton";
      componentResourceManager.ApplyResources((object) this.azureButton, "azureButton");
      this.azureButton.ToggleGroupName = "color";
      this.azureButton.PressedButtonChanged += new EventHandler(this.themeColor_PressedButtonChanged);
      this.blueButton.CanDepress = false;
      this.blueButton.Name = "blueButton";
      componentResourceManager.ApplyResources((object) this.blueButton, "blueButton");
      this.blueButton.ToggleGroupName = "color";
      this.greenButton.CanDepress = false;
      this.greenButton.Name = "greenButton";
      componentResourceManager.ApplyResources((object) this.greenButton, "greenButton");
      this.greenButton.ToggleGroupName = "color";
      this.orangeButton.CanDepress = false;
      this.orangeButton.Name = "orangeButton";
      componentResourceManager.ApplyResources((object) this.orangeButton, "orangeButton");
      this.orangeButton.ToggleGroupName = "color";
      this.orchidButton.CanDepress = false;
      this.orchidButton.Name = "orchidButton";
      componentResourceManager.ApplyResources((object) this.orchidButton, "orchidButton");
      this.orchidButton.ToggleGroupName = "color";
      this.redButton.CanDepress = false;
      this.redButton.Name = "redButton";
      componentResourceManager.ApplyResources((object) this.redButton, "redButton");
      this.redButton.ToggleGroupName = "color";
      this.tealButton.CanDepress = false;
      this.tealButton.Name = "tealButton";
      componentResourceManager.ApplyResources((object) this.tealButton, "tealButton");
      this.tealButton.ToggleGroupName = "color";
      this.violetButton.CanDepress = false;
      this.violetButton.Name = "violetButton";
      componentResourceManager.ApplyResources((object) this.violetButton, "violetButton");
      this.violetButton.ToggleGroupName = "color";
      this.themeLightnessMenu.Items.Add((RibbonItem) this.darkGrayButton);
      this.themeLightnessMenu.Items.Add((RibbonItem) this.lightGrayButton);
      this.themeLightnessMenu.Items.Add((RibbonItem) this.whiteButton);
      this.themeLightnessMenu.Name = "themeLightnessMenu";
      componentResourceManager.ApplyResources((object) this.themeLightnessMenu, "themeLightnessMenu");
      this.darkGrayButton.CanDepress = false;
      this.darkGrayButton.Name = "darkGrayButton";
      componentResourceManager.ApplyResources((object) this.darkGrayButton, "darkGrayButton");
      this.darkGrayButton.ToggleGroupName = "lightness";
      this.darkGrayButton.PressedButtonChanged += new EventHandler(this.themeLightness_PressedButtonChanged);
      this.lightGrayButton.CanDepress = false;
      this.lightGrayButton.Name = "lightGrayButton";
      componentResourceManager.ApplyResources((object) this.lightGrayButton, "lightGrayButton");
      this.lightGrayButton.ToggleGroupName = "lightness";
      this.whiteButton.CanDepress = false;
      this.whiteButton.Name = "whiteButton";
      componentResourceManager.ApplyResources((object) this.whiteButton, "whiteButton");
      this.whiteButton.ToggleGroupName = "lightness";
      this.ribbonSeparator7.Name = "ribbonSeparator7";
      this.customButton.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.customButton, "customButton");
      this.customButton.Name = "customButton";
      this.customButton.ToggleGroupName = "visualStyle";
      this.blue2007Button.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.blue2007Button, "blue2007Button");
      this.blue2007Button.Name = "blue2007Button";
      this.blue2007Button.ToggleGroupName = "visualStyle";
      this.blue2007Button.Visible = false;
      this.blue2007Button.PressedButtonChanged += new EventHandler(this.visualStyle_PressedButtonChanged);
      this.silver2007Button.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.silver2007Button, "silver2007Button");
      this.silver2007Button.Name = "silver2007Button";
      this.silver2007Button.ToggleGroupName = "visualStyle";
      this.silver2007Button.Visible = false;
      this.black2007Button.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.black2007Button, "black2007Button");
      this.black2007Button.Name = "black2007Button";
      this.black2007Button.ToggleGroupName = "visualStyle";
      this.black2007Button.Visible = false;
      this.blue2010Button.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.blue2010Button, "blue2010Button");
      this.blue2010Button.Name = "blue2010Button";
      this.blue2010Button.ToggleGroupName = "visualStyle";
      this.silver2010Button.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.silver2010Button, "silver2010Button");
      this.silver2010Button.Name = "silver2010Button";
      this.silver2010Button.ToggleGroupName = "visualStyle";
      this.black2010Button.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.black2010Button, "black2010Button");
      this.black2010Button.Name = "black2010Button";
      this.black2010Button.ToggleGroupName = "visualStyle";
      this.windows7Button.CanDepress = false;
      componentResourceManager.ApplyResources((object) this.windows7Button, "windows7Button");
      this.windows7Button.Name = "windows7Button";
      this.windows7Button.ToggleGroupName = "visualStyle";
      this.appMenu.BottomPaneItems.Add((RibbonItem) this.exitButton);
      this.appMenu.ColoredButton = ColoredButton.Green;
      componentResourceManager.ApplyResources((object) this.appMenu, "appMenu");
      this.appMenu.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.ApplicationButton;
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileNewProject);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileOpenProject);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileOpenSampleProject);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileCreateReinventory);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileSaveProject);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileSaveProjectAs);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFilePackProject);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFilePrint);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileExport);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileImport);
      this.appMenu.LeftPaneItems.Add((RibbonItem) this.mnuFileCloseProject);
      this.appMenu.Name = "appMenu";
      this.appMenu.RightPaneItems.Add((RibbonItem) this.rmRecentProjects);
      this.appMenu.RightPaneItems.Add((RibbonItem) this.rliSeparartor);
      this.exitButton.Name = "exitButton";
      this.exitButton.SmallImage = (Image) componentResourceManager.GetObject("exitButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.exitButton, "exitButton");
      this.exitButton.Click += new EventHandler(this.exitButton_Click);
      this.mnuFileNewProject.LargeImage = (Image) componentResourceManager.GetObject("mnuFileNewProject.LargeImage");
      this.mnuFileNewProject.Name = "mnuFileNewProject";
      this.mnuFileNewProject.SmallImage = (Image) componentResourceManager.GetObject("mnuFileNewProject.SmallImage");
      componentResourceManager.ApplyResources((object) this.mnuFileNewProject, "mnuFileNewProject");
      this.mnuFileNewProject.Click += new EventHandler(this.mnuFileNewProject_Click);
      this.mnuFileOpenProject.LargeImage = (Image) componentResourceManager.GetObject("mnuFileOpenProject.LargeImage");
      this.mnuFileOpenProject.Name = "mnuFileOpenProject";
      this.mnuFileOpenProject.SmallImage = (Image) componentResourceManager.GetObject("mnuFileOpenProject.SmallImage");
      componentResourceManager.ApplyResources((object) this.mnuFileOpenProject, "mnuFileOpenProject");
      this.mnuFileOpenProject.Click += new EventHandler(this.mnuFileOpenProject_Click);
      this.mnuFileOpenSampleProject.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.load_file_folder_Large;
      this.mnuFileOpenSampleProject.Name = "mnuFileOpenSampleProject";
      componentResourceManager.ApplyResources((object) this.mnuFileOpenSampleProject, "mnuFileOpenSampleProject");
      this.mnuFileOpenSampleProject.Click += new EventHandler(this.mnuFileOpenSampleProject_Click);
      this.mnuFileCreateReinventory.Enabled = false;
      this.mnuFileCreateReinventory.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.load_file_folder_Large;
      this.mnuFileCreateReinventory.Name = "mnuFileCreateReinventory";
      componentResourceManager.ApplyResources((object) this.mnuFileCreateReinventory, "mnuFileCreateReinventory");
      this.mnuFileCreateReinventory.Click += new EventHandler(this.mnuFileCreateReinventory_Click);
      this.mnuFileSaveProject.Enabled = false;
      this.mnuFileSaveProject.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.save_project_Large;
      this.mnuFileSaveProject.Name = "mnuFileSaveProject";
      componentResourceManager.ApplyResources((object) this.mnuFileSaveProject, "mnuFileSaveProject");
      this.mnuFileSaveProjectAs.Enabled = false;
      this.mnuFileSaveProjectAs.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.save_project_as_Large;
      this.mnuFileSaveProjectAs.Name = "mnuFileSaveProjectAs";
      componentResourceManager.ApplyResources((object) this.mnuFileSaveProjectAs, "mnuFileSaveProjectAs");
      this.mnuFileSaveProjectAs.Click += new EventHandler(this.mnuFileSaveProjectAs_Click);
      this.mnuFilePackProject.Enabled = false;
      this.mnuFilePackProject.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.pack_project_Large;
      this.mnuFilePackProject.Name = "mnuFilePackProject";
      componentResourceManager.ApplyResources((object) this.mnuFilePackProject, "mnuFilePackProject");
      this.mnuFilePackProject.Click += new EventHandler(this.mnuFilePackProject_Click);
      this.mnuFilePrint.LargeImage = (Image) componentResourceManager.GetObject("mnuFilePrint.LargeImage");
      this.mnuFilePrint.Name = "mnuFilePrint";
      componentResourceManager.ApplyResources((object) this.mnuFilePrint, "mnuFilePrint");
      this.mnuFilePrint.Visible = false;
      this.mnuFileExport.LargeImage = (Image) componentResourceManager.GetObject("mnuFileExport.LargeImage");
      this.mnuFileExport.Name = "mnuFileExport";
      componentResourceManager.ApplyResources((object) this.mnuFileExport, "mnuFileExport");
      this.mnuFileExport.Visible = false;
      this.mnuFileImport.LargeImage = (Image) componentResourceManager.GetObject("mnuFileImport.LargeImage");
      this.mnuFileImport.Name = "mnuFileImport";
      componentResourceManager.ApplyResources((object) this.mnuFileImport, "mnuFileImport");
      this.mnuFileImport.Visible = false;
      this.mnuFileCloseProject.Enabled = false;
      this.mnuFileCloseProject.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.close_project_Large;
      this.mnuFileCloseProject.Name = "mnuFileCloseProject";
      componentResourceManager.ApplyResources((object) this.mnuFileCloseProject, "mnuFileCloseProject");
      this.mnuFileCloseProject.Click += new EventHandler(this.mnuFileCloseProject_Click);
      this.rmRecentProjects.AllowSelection = false;
      this.rmRecentProjects.Items.Add((RibbonItem) this.rlProjectsLabel);
      this.rmRecentProjects.Name = "rmRecentProjects";
      this.rlProjectsLabel.Name = "rlProjectsLabel";
      componentResourceManager.ApplyResources((object) this.rlProjectsLabel, "rlProjectsLabel");
      this.rliSeparartor.Items.Add((RibbonItem) this.rsRecentProjects);
      this.rliSeparartor.Name = "rliSeparartor";
      this.rsRecentProjects.Name = "rsRecentProjects";
      this.undoSplitButton.Name = "undoSplitButton";
      this.undoSplitButton.SmallImage = (Image) componentResourceManager.GetObject("undoSplitButton.SmallImage");
      componentResourceManager.ApplyResources((object) this.undoSplitButton, "undoSplitButton");
      this.qat.HotItemLinks.Add((Component) this.undoSplitButton);
      this.qat.Name = "qat";
      this.qat.Visible = false;
      this.c1Ribbon1.AllowMinimize = false;
      this.c1Ribbon1.ApplicationMenuHolder = this.appMenu;
      this.c1Ribbon1.ConfigToolBarHolder = this.configToolBar;
      componentResourceManager.ApplyResources((object) this.c1Ribbon1, "c1Ribbon1");
      this.c1Ribbon1.Location = new Point(0, 0);
      this.c1Ribbon1.Name = "c1Ribbon1";
      this.c1Ribbon1.QatHolder = this.qat;
      this.c1Ribbon1.QatItemsHolder.Add((RibbonItem) this.undoSplitButton);
      this.c1Ribbon1.Size = new Size(1791, 223);
      this.c1Ribbon1.Tabs.Add(this.rtProject);
      this.c1Ribbon1.Tabs.Add(this.rtData);
      this.c1Ribbon1.Tabs.Add(this.rtView);
      this.c1Ribbon1.Tabs.Add(this.rtReports);
      this.c1Ribbon1.Tabs.Add(this.rtForecast);
      this.c1Ribbon1.Tabs.Add(this.rtSupport);
      this.c1Ribbon1.SelectedTabChanged += new EventHandler(this.c1Ribbon1_SelectedTabChanged);
      this.c1Ribbon1.VisualStyleChanged += new EventHandler(this.c1Ribbon1_VisualStyleChanged);
      this.c1Ribbon1.MinimizedChanged += new EventHandler(this.c1Ribbon1_MinimizedChanged);
      this.rtView.Groups.Add(this.dataEntryGroup);
      this.rtView.Groups.Add(this.rgPlot);
      this.rtView.Groups.Add(this.rgSpeciesName);
      this.rtView.Groups.Add(this.rgUnits);
      this.rtView.Groups.Add(this.rgSpecies);
      this.rtView.Groups.Add(this.rgViewExport);
      this.rtView.Groups.Add(this.rgConfiguration);
      this.rtView.Name = "rtView";
      componentResourceManager.ApplyResources((object) this.rtView, "rtView");
      this.dataEntryGroup.Items.Add((RibbonItem) this.rcboProject);
      this.dataEntryGroup.Items.Add((RibbonItem) this.rcboSeries);
      this.dataEntryGroup.Items.Add((RibbonItem) this.rcboYear);
      this.dataEntryGroup.Name = "dataEntryGroup";
      componentResourceManager.ApplyResources((object) this.dataEntryGroup, "dataEntryGroup");
      this.rcboProject.DropDownStyle = RibbonComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.rcboProject, "rcboProject");
      this.rcboProject.Name = "rcboProject";
      this.rcboProject.SupportedGroupSizing = SupportedGroupSizing.TextAlwaysVisible;
      this.rcboProject.SelectedIndexChanged += new EventHandler(this.rcboProject_SelectedIndexChanged);
      this.rcboSeries.DropDownStyle = RibbonComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.rcboSeries, "rcboSeries");
      this.rcboSeries.Name = "rcboSeries";
      this.rcboSeries.SupportedGroupSizing = SupportedGroupSizing.TextAlwaysVisible;
      this.rcboSeries.SelectedIndexChanged += new EventHandler(this.rcboSeries_SelectedIndexChanged);
      this.rcboYear.DropDownStyle = RibbonComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.rcboYear, "rcboYear");
      this.rcboYear.Name = "rcboYear";
      this.rcboYear.SupportedGroupSizing = SupportedGroupSizing.TextAlwaysVisible;
      this.rcboYear.SelectedIndexChanged += new EventHandler(this.rcboYear_SelectedIndexChanged);
      this.rgSpeciesName.Items.Add((RibbonItem) this.rbSpeciesCN);
      this.rgSpeciesName.Items.Add((RibbonItem) this.rbSpeciesSN);
      this.rgSpeciesName.Name = "rgSpeciesName";
      componentResourceManager.ApplyResources((object) this.rgSpeciesName, "rgSpeciesName");
      this.rbSpeciesCN.CanDepress = false;
      this.rbSpeciesCN.Name = "rbSpeciesCN";
      this.rbSpeciesCN.Pressed = true;
      componentResourceManager.ApplyResources((object) this.rbSpeciesCN, "rbSpeciesCN");
      this.rbSpeciesCN.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbSpeciesCN.ToggleGroupName = "tgSpecies";
      this.rbSpeciesCN.PressedChanged += new EventHandler(this.rgSpecies_PressedChanged);
      this.rbSpeciesCN.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgSpecies_MeasureItem);
      this.rbSpeciesCN.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgSpecies_DrawItem);
      this.rbSpeciesSN.CanDepress = false;
      this.rbSpeciesSN.Name = "rbSpeciesSN";
      componentResourceManager.ApplyResources((object) this.rbSpeciesSN, "rbSpeciesSN");
      this.rbSpeciesSN.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbSpeciesSN.ToggleGroupName = "tgSpecies";
      this.rbSpeciesSN.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgSpecies_MeasureItem);
      this.rbSpeciesSN.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgSpecies_DrawItem);
      this.rgUnits.Items.Add((RibbonItem) this.rbEnglish);
      this.rgUnits.Items.Add((RibbonItem) this.rbMetric);
      this.rgUnits.Name = "rgUnits";
      componentResourceManager.ApplyResources((object) this.rgUnits, "rgUnits");
      this.rbEnglish.CanDepress = false;
      this.rbEnglish.Name = "rbEnglish";
      this.rbEnglish.Pressed = true;
      componentResourceManager.ApplyResources((object) this.rbEnglish, "rbEnglish");
      this.rbEnglish.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbEnglish.ToggleGroupName = "tgUnit";
      this.rbEnglish.PressedChanged += new EventHandler(this.rgUnits_PressedChanged);
      this.rbEnglish.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgUnits_MeasureItem);
      this.rbEnglish.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgUnits_DrawItem);
      this.rbMetric.CanDepress = false;
      this.rbMetric.Name = "rbMetric";
      componentResourceManager.ApplyResources((object) this.rbMetric, "rbMetric");
      this.rbMetric.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
      this.rbMetric.ToggleGroupName = "tgUnit";
      this.rbMetric.MeasureItem += new C1.Win.C1Ribbon.MeasureItemEventHandler(this.rgUnits_MeasureItem);
      this.rbMetric.DrawItem += new C1.Win.C1Ribbon.DrawItemEventHandler(this.rgUnits_DrawItem);
      this.rgSpecies.Items.Add((RibbonItem) this.rbtnSpeciesCode);
      this.rgSpecies.Items.Add((RibbonItem) this.rbSpeciesList);
      this.rgSpecies.Name = "rgSpecies";
      componentResourceManager.ApplyResources((object) this.rgSpecies, "rgSpecies");
      this.rbtnSpeciesCode.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.specifis_codes_Large;
      this.rbtnSpeciesCode.Name = "rbtnSpeciesCode";
      componentResourceManager.ApplyResources((object) this.rbtnSpeciesCode, "rbtnSpeciesCode");
      this.rbtnSpeciesCode.Click += new EventHandler(this.rbtnSpeciesCode_Click);
      this.rbSpeciesList.LargeImage = (Image) componentResourceManager.GetObject("rbSpeciesList.LargeImage");
      this.rbSpeciesList.Name = "rbSpeciesList";
      this.rbSpeciesList.SmallImage = (Image) componentResourceManager.GetObject("rbSpeciesList.SmallImage");
      componentResourceManager.ApplyResources((object) this.rbSpeciesList, "rbSpeciesList");
      this.rbSpeciesList.Click += new EventHandler(this.rbSpeciesList_Click);
      this.rgViewExport.Enabled = false;
      this.rgViewExport.Items.Add((RibbonItem) this.rbViewExportCSV);
      this.rgViewExport.Name = "rgViewExport";
      componentResourceManager.ApplyResources((object) this.rgViewExport, "rgViewExport");
      this.rbViewExportCSV.LargeImage = (Image) i_Tree_Eco_v6.Properties.Resources.csv_Large;
      this.rbViewExportCSV.Name = "rbViewExportCSV";
      this.rbViewExportCSV.SmallImage = (Image) i_Tree_Eco_v6.Properties.Resources.csv_Small;
      componentResourceManager.ApplyResources((object) this.rbViewExportCSV, "rbViewExportCSV");
      this.rbViewExportCSV.Click += new EventHandler(this.rbExportToCSV_Click);
      this.rgConfiguration.Items.Add((RibbonItem) this.colorSchemeMenu);
      this.rgConfiguration.Items.Add((RibbonItem) this.rcbAutoCheckUpdates);
      this.rgConfiguration.Items.Add((RibbonItem) this.rcbMinimizeHelp);
      this.rgConfiguration.Name = "rgConfiguration";
      componentResourceManager.ApplyResources((object) this.rgConfiguration, "rgConfiguration");
      this.rcbAutoCheckUpdates.Name = "rcbAutoCheckUpdates";
      componentResourceManager.ApplyResources((object) this.rcbAutoCheckUpdates, "rcbAutoCheckUpdates");
      this.rcbAutoCheckUpdates.CheckedChanged += new EventHandler(this.rcbAutoCheckUpdates_CheckedChanged);
      this.rcbMinimizeHelp.Name = "rcbMinimizeHelp";
      componentResourceManager.ApplyResources((object) this.rcbMinimizeHelp, "rcbMinimizeHelp");
      this.rcbMinimizeHelp.CheckedChanged += new EventHandler(this.rcbMinimizeHelp_CheckedChanged);
      this.ribbonGroup8.Name = "ribbonGroup8";
      componentResourceManager.ApplyResources((object) this.ribbonGroup8, "ribbonGroup8");
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dockPanel1);
      this.Controls.Add((Control) this.c1StatusBar1);
      this.Controls.Add((Control) this.c1Ribbon1);
      this.IsMdiContainer = true;
      this.Name = nameof (MainRibbonForm);
      this.Opacity = 0.0;
      this.VisualStyleHolder = VisualStyle.Windows7;
      this.FormClosed += new FormClosedEventHandler(this.MainRibbonForm_FormClosed);
      this.Load += new EventHandler(this.MainRibbonForm_Load);
      ((ISupportInitialize) this.c1StatusBar1).EndInit();
      ((ISupportInitialize) this.c1Ribbon1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
