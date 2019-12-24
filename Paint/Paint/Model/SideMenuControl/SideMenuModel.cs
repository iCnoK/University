using Microsoft.Win32;

namespace Paint.Model.SideMenuControl
{
    public static class SideMenuModel
    {
        public static OpenFileDialog InitOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "tiff files (*.tiff)|*.tiff|png files (*.png)|*.png|bmp files (*.bmp)|*.bmp|jpeg files (*.jpeg)|*.jpeg";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            return openFileDialog;
        }

        public static SaveFileDialog InitSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "tiff files (*.tiff)|*.tiff|png files (*.png)|*.png|bmp files (*.bmp)|*.bmp|jpeg files (*.jpeg)|*.jpeg";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }
    }
}
