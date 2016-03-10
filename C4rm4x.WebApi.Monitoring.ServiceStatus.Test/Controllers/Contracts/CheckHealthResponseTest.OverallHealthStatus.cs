#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers.Contracts
{
    public partial class CheckHealthResponseTest
    {
        [TestClass]
        public class CheckHealthResponseOverallHealthStatusTest
        {
            [TestMethod, UnitTest]
            public void OverallHealthStatus_Returns_SystemHealthStatus_Healthy_When_All_Component_HealthStatus_Are_ComponentHealthStatus_Working()
            {
                Assert.AreEqual(
                    SystemHealthStatus.Healthy,
                    CreateSubjectUnderTest(GetComponentStatusAsWorking)
                        .OverallHealthStatus);
            }

            [TestMethod, UnitTest]
            public void OverallHealthStatus_Returns_SystemHealthStatus_Disaster_When_All_Component_HealthStatus_Are_ComponentHealthStatus_Unresponsive()
            {
                Assert.AreEqual(
                    SystemHealthStatus.Disaster,
                    CreateSubjectUnderTest(GetComponentStatusAsUnresponsive)
                        .OverallHealthStatus);
            }
            
            [TestMethod, UnitTest]            
            public void OverallHealthStatus_Returns_SystemHealthStatus_WithIssues_When_All_Component_HealthStatus_Are_Neither_ComponentHealthStatus_Working_Nor_ComponentHealthStatus_Unresponsive()
            {
                Assert.AreEqual(
                    SystemHealthStatus.WithIssues,
                    CreateSubjectUnderTest(
                        GetComponentStatuses(GetRand(0, 5), GetComponentStatusAsWorking)
                            .Union(GetComponentStatuses(GetRand(0, 5), GetComponentStatusAsUnresponsive))
                            .Union(GetComponentStatuses(GetRand(1, 5), GetComponentStatusAsUnknown)))
                        .OverallHealthStatus);
            }

            private static CheckHealthResponse CreateSubjectUnderTest(
                Func<ComponentStatusDto> componentStatusGenerator)
            {
                return CreateSubjectUnderTest(
                    GetComponentStatuses(componentStatusGenerator).ToList());
            }

            private static CheckHealthResponse CreateSubjectUnderTest(
                IEnumerable<ComponentStatusDto> componentStatuses)
            {
                return new CheckHealthResponse(componentStatuses);
            }

            private static IEnumerable<ComponentStatusDto> GetComponentStatuses(
                Func<ComponentStatusDto> componentStatusGenerator)
            {
                return GetComponentStatuses(GetRand(1, 10), componentStatusGenerator);
            }

            private static IEnumerable<ComponentStatusDto> GetComponentStatuses(
                int numberOfComponentStatus,
                Func<ComponentStatusDto> componentStatusGenerator)
            {
                for (var i = 0; i < numberOfComponentStatus; i++)
                    yield return componentStatusGenerator();
            }

            private static int GetRand(int min, int max)
            {
                return new Random().Next(min, max);
            }

            private static ComponentStatusDto GetComponentStatusAsWorking()
            {
                return GetComponentStatusAs(ComponentHealthStatus.Working);
            }

            private static ComponentStatusDto GetComponentStatusAsUnresponsive()
            {
                return GetComponentStatusAs(ComponentHealthStatus.Unresponsive);
            }

            public static ComponentStatusDto GetComponentStatusAsUnknown()
            {
                return GetComponentStatusAs(ComponentHealthStatus.Unknown);
            }

            private static ComponentStatusDto GetComponentStatusAs(ComponentHealthStatus healthStatus)
            {
                return new ComponentStatusDtoBuilder()
                    .WithHealthStatus(healthStatus)
                    .Build();
            }
        }
    }
}
