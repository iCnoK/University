//using Paint.View;
//using Paint.ViewModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Paint.Utility
//{
//    public class MessageBoxService
//    {
//        public MessageBoxViewModel MessageBoxViewModel
//        {
//            get => MessageBox.DataContext as MessageBoxViewModel;
//        }
//        MessageBox MessageBox { get; set; }

//        public void Show()
//        {
//            MessageBox.Show();
//        }

//        public MessageBoxService(int height, int width, string title, string text)
//        {
//            MessageBox = new MessageBox();
//            MessageBox.DataContext = new MessageBoxViewModel(height, width, title, text);
//        }
//    }
//}
