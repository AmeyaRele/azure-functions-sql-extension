// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Azure.WebJobs.Extensions.Sql
{
    public static class SqlTriggerConstants
    {
        // Unit of time is seconds
        public const string LeaseUnits = "s";

        public const string ScaleControllerPollingIntervalUnits = "s";

        public const long MinimumNumberOfSamples = 5;

        public const long MinimumPercentIncrease = 25;

        public const long MinimumPercentDecrease = 25;

        public const int MaxDequeueCount = 5;

        public const int LeaseInterval = 30;

        public const int PollingInterval = 5;

        public const int CleanupInterval = 300;

        // Unit of time is seconds
        public const string CleanupUnits = "s";

        public const int MaxLeaseRenewalCount = 5;

    }
}