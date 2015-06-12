using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRRDirectoryInstall
{
    public enum AzureGeoRegions
    {
        [Description("East US")]
        EAST_US,
        [Description("West US")]
        WEST_US,
        [Description("North Central US")]
        NORTH_CENTRAL_US,
        [Description("North Europe")]
        NORTH_EUROPE,
        [Description("West Europe")]
        WEST_EUROPE,
        [Description("East Asia")]
        EAST_ASIA
    }

    public enum AzureWebSpaces
    {
        EASTUSWEBSPACE,
        WESTUSWEBSPACE,
        NORTHCENTRALUSWEBSPACE,
        NORTHEUROPEWEBSPACE,
        WESTEURPEWEBSPACE,
        EASTASIAWEBSPACE
    }
}
