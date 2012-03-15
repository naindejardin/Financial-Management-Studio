using FMStudio.Application.Test.Views;
using FMStudio.Application.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Application.Test.ViewModels
{
    [TestClass]
    public class DialogViewModelTest
    {
        [TestMethod]
        public void DialogViewModelCloseTest()
        {
            MockDialogView view = new MockDialogView();
            MockDialogViewModel viewModel = new MockDialogViewModel(view);

            // In this case it tries to get the title of the unit test framework which is ""
            Assert.AreEqual("", MockDialogViewModel.Title);

            object owner = new object();
            Assert.IsFalse(view.IsVisible);
            view.ShowDialogAction = v =>
            {
                Assert.AreEqual(owner, v.Owner);
                Assert.IsTrue(v.IsVisible);
            };
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.IsNull(dialogResult);
            Assert.IsFalse(view.IsVisible);

            view.ShowDialogAction = v =>
            {
                viewModel.YesCommand.Execute(null);
            };
            dialogResult = viewModel.ShowDialog(owner);
            Assert.AreEqual(true, dialogResult);

            view.ShowDialogAction = v =>
            {
                viewModel.NoCommand.Execute(null);
            };
            dialogResult = viewModel.ShowDialog(owner);
            Assert.AreEqual(false, dialogResult);
        }


        private class MockDialogViewModel : DialogViewModel<MockDialogView>
        {
            public MockDialogViewModel(MockDialogView view)
                : base(view)
            {
            }
        }
    }
}

