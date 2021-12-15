using System;
using System.Windows;

namespace DirectoryManagerTsp
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// メインのプラグラム
        /// </summary>
        [STAThread()]
        static public void Main()
        {
            try
            {
                var settingManager = Setting.ClassIntegrationManager.Instance;

                // 設定ファイルを読み込み
                settingManager.LoadIntegration(Shared.Tool.ClassPath.DirectoryDelimiter("."));
            }
            catch (Exception ex)
            {
                // エラーのログを出力
                Shared.Tool.ClassException.Logging(ex);
            }

            try
            {
                var settingManager = Setting.ClassIntegrationManager.Instance;
                var integration = settingManager.GetIntegration();
                var workerManager = Shared.Setting.ClassWorkerManager.Instance;

                // 設定ファイルを読み込み
                workerManager.Load(integration.WorkerPath);
            }
            catch (Exception ex)
            {
                // エラーのログを出力
                Shared.Tool.ClassException.Logging(ex);
            }

            try
            {
                var app = new App
                {
                    // メインのウィンドウを設定
                    StartupUri = new Uri("WindowMain.xaml", UriKind.Relative)
                };

                app.InitializeComponent();

                app.Run();
            }
            catch (Exception ex)
            {
                // エラーのログを出力
                Shared.Tool.ClassException.Logging(ex);
            }

            try
            {
                var localManager = Setting.ClassLocalManager.Instance;

                // 設定を保存
                localManager.Save();
            }
            catch (Exception ex)
            {
                // エラーのログを出力
                Shared.Tool.ClassException.Logging(ex);
            }
        }
    }
}
