using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCC_MVVM.ViewModel.Job;
using TCC_MVVM.Model;
using System.Windows.Input;
using TCC_MVVM.ViewModel;

namespace TCC_MVVM_Test
{
    [TestClass]
    public class JobVM_Test
    {
        [TestMethod]
        public void CreateJobCommand_Test()
        {
            StripVM viewmodel = new StripVM();

            Strip strip01 = new Strip();
            Strip strip02 = new Strip();

            viewmodel.Strips.Add(new Strip());
        }
    }
}
