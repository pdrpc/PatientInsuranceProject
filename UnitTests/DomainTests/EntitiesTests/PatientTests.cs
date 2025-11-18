using Domain.Entities;

namespace PatientInsuranceProject.UnitTests.Domain.Entities
{
    public class PatientTests
    {
        [Fact]
        public void Patient_Constructor_ShouldSetIsActiveToTrue()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.True(patient.IsActive);
        }

        [Fact]
        public void Patient_Constructor_ShouldAssignANewGuid()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotEqual(Guid.Empty, patient.Id);
        }

        [Fact]
        public void Patient_Constructor_MultipleInstances_ShouldHaveDifferentIds()
        {
            // Arrange & Act
            var patient1 = new Patient();
            var patient2 = new Patient();

            // Assert
            Assert.NotEqual(patient1.Id, patient2.Id);
        }
    }
}