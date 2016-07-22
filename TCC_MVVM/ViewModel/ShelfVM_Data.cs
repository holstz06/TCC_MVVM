using TCC_MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;
using System;
namespace TCC_MVVM.ViewModel
{
    public partial class ShelfVM
    {
        DataTable ShelfData;
        DataTable ShelvingDepthData;
        DataTable ShelfWidthData;
        DataTable WoodData;
        DataTable BandingData;
        DataTable ShelfTypeData;

        List<Campost> CamPosts = new List<Campost>();
        List<Fence> Fences = new List<Fence>();
        List<TopConnector> TopConnectors = new List<TopConnector>();
        List<ShelfType> ShelfTypes = new List<ShelfType>();

        Dictionary<string, decimal> Woodvalues;
        Dictionary<string, decimal> BandingValues;

        /// <summary>
        /// Initializes the data tables
        /// </summary>
        public void InitializeData()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml("ShelfData.xml");
            ShelfData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelfWidthData.xml");
            ShelfWidthData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelvingDepthData.xml");
            ShelvingDepthData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("WoodData.xml");
            WoodData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("BandingData.xml");
            BandingData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelfTypeData.xml");
            ShelfTypeData = dataset.Tables[0];

            // Get shelf related items from datasets
            CamPosts = GetCamPosts();
            Fences = GetFences();
            ShelfTypes = GetShelfTypes();
            TopConnectors = GetTopConnectors();
            Woodvalues = GetWoodValues();
            BandingValues = GetBandingValues();
        }

        /// <summary>
        /// Gets a list of color values
        /// </summary>
        List<string> GetColorValues()
            => WoodData.AsEnumerable().Select(row => row.Field<string>("WoodColor")).Distinct().ToList();

        /// <summary>
        /// Gets a list of width values
        /// </summary>
        List<string> GetWidthValues()
            => ShelfWidthData.AsEnumerable().Select(row => row.Field<string>("ShelfWidth")).Distinct().ToList();

        /// <summary>
        /// Gets a list of depth values
        /// </summary>
        List<string> GetDepthValues()
            => ShelvingDepthData.AsEnumerable().Select(row => row.Field<string>("ShelvingDepth")).Distinct().ToList();

        /// <summary>
        /// Gets a list of shelf type names
        /// </summary>
        /// <returns></returns>
        List<string> GetShelfTypeNames()
            => ShelfTypeData.AsEnumerable().Select(row => row.Field<string>("ShelfType")).Distinct().ToList();

        /// <summary>
        /// Gets a list of wood values
        /// </summary>
        Dictionary<string, decimal> GetWoodValues()
            => WoodData.AsEnumerable().ToDictionary(row => row.Field<string>("WoodColor"), row => decimal.Parse(row.Field<string>("WoodPrice")));

        /// <summary>
        /// Gets a list of banding values
        /// </summary>
        Dictionary<string, decimal> GetBandingValues()
            => BandingData.AsEnumerable().ToDictionary(row => row.Field<string>("BandingColor"), row => decimal.Parse(row.Field<string>("BandingPrice")));

        /// <summary>
        /// Retrieves a list of shelf types
        /// </summary>
        List<ShelfType> GetShelfTypes()
            => ShelfTypeData.AsEnumerable().Select(row => new ShelfType
            {
                Name            = row.Field<string>("ShelfType"),
                CamPostQuantity = int.Parse(row.Field<string>("CamQuantity")),
                HasFence        = bool.Parse(row.Field<string>("HasFencePost")),
                FenceColor      = row.Field<string>("FencePostColor"),
                HasTopConnector = bool.Parse(row.Field<string>("HasTopConnector")),
                IsToeKick       = bool.Parse(row.Field<string>("IsToeKick"))
                
            }).ToList();

        /// <summary>
        /// Retrieves a list of camposts
        /// </summary>
        List<Campost> GetCamPosts()
            => (from row in ShelfData.AsEnumerable()
                where row.Field<string>("ItemName") == "Cam Post"
                select new Campost
                {
                    WoodColor = row.Field<string>("WoodColor"),
                    Color = row.Field<string>("Color"),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();

        /// <summary>
        /// Retrieves a list of fences
        /// </summary>
        List<Fence> GetFences()
            => (from row in ShelfData.AsEnumerable()
                where row.Field<string>("ItemName") == "Fence"
                select new Fence
                {
                    Color = row.Field<string>("Color"),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();

        /// <summary>
        /// Gets a list of top connector items
        /// </summary>
        List<TopConnector> GetTopConnectors()
            => (from row in ShelfData.AsEnumerable()
                where row.Field<string>("ItemName") == "Top Connector"
                select new TopConnector
                {
                    WoodColor = row.Field<string>("WoodColor"),
                    Color = row.Field<string>("Color"),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();
    }
}
