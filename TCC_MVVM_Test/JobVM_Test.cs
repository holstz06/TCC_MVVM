using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;
using TCC_MVVM.ViewModel.Job;
using TCC_MVVM.ViewModel.Room;

namespace TCC_MVVM_Test
{
    [TestClass]
    public class JobVM_Test
    {
        /// <summary>
        /// The job view model
        /// </summary>
        JobVM job = new JobVM();

        /// <summary>
        /// The room view model (also contains the room model)
        /// </summary>
        RoomVM room;

        /// <summary>
        /// Attempt to create a new room
        /// </summary>
        [TestMethod]
        public void AddRoomToJob()
        {
            room = job.AddRoom();
        }

        /// <summary>
        /// Attempt to remove that room that was created
        /// </summary>
        [TestMethod]
        public void RemoveRoomFromJob()
        {
            if(room != null)
                job.Remove(room);
        }
    }
}
