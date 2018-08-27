using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.IPTVServiceV7;

namespace Common.ApMax
{
    public class ApMaxHelper
    {

        public  List<IPTVAccountType> FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(List<IPTVAccountType> iptvAccountTypes)
        {
            bool isMissingPackageNames = false;

            //Having issues with nulls... So explicitly checking for nulls.
            if (iptvAccountTypes != null && iptvAccountTypes.Any())
            {
                foreach (var iptvAccountType in iptvAccountTypes)
                {
                    if (iptvAccountType != null && iptvAccountType.ChannelPackageList != null && iptvAccountType.ChannelPackageList.Any())
                    {
                        isMissingPackageNames = (
                            from y in iptvAccountType.ChannelPackageList
                            where (y.PackageName == null || y.PackageName.Trim() == "")
                            select y
                            ).Any();
                    }
                }
            }

            if (isMissingPackageNames)
            {
                //iptvAccountTypes = PopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);
            }

            return iptvAccountTypes;
        }
    }
}
