using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Controller
{
    public static class MarketplaceController
    {
        public enum MarketplaceTypes
        {
            Ozon,
            WB
        }

        public static string GetMarketplace(int marketplaceId)
        {
            switch (marketplaceId)
            {
                case (int)MarketplaceTypes.WB:
                    return "WB";
                case (int)MarketplaceTypes.Ozon:
                    return "OZON";
                default:
                    return string.Empty;
            }
        }
    }
}
