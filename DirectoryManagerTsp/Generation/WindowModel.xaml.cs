using System;
using System.IO;
using System.Windows;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        /// <summary>
        /// WindowModel.xaml の相互作用ロジック
        /// </summary>
        public partial class WindowModel : Window
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WindowModel()
            {
                InitializeComponent();

                try
                {
                    // 機種名のヘッダを更新
                    labelNameHeader.Content = Setting.ClassIntegration.ModelHeader;
                }
                catch (Exception ex)
                {
                    // エラーのメッセージを表示
                    Shared.Tool.ClassException.Message(ex);
                }
            }

            /// <summary>
            /// 作成ボタンクリック
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void buttonGeneration_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    // 機種名を取得
                    var name = labelNameHeader.Content + textBoxName.Text;

                    //@@@ 機種名の規則を確認する

                    var settingManager = Setting.ClassIntegrationManager.Instance;
                    var integration = settingManager.GetIntegration();
                    var path = "";

                    // テンプレートのルートディレクトリ
                    path = integration.TemplateRootDirectory;

                    // ディレクトリの区切りを付加
                    path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                    // 機種名を付加
                    path += name;

                    // 機種名のディレクトリを確認
                    if (Directory.Exists(path))
                    {
                        // 機種名のディレクトリが存在する ⇒ 例外を発砲
                        throw new Exception(name + "は登録済です。");
                    }

                    // 部署を走査
                    foreach (var department in integration.Departments)
                    {
                        // 部署のディレクトリ内に機種のディレクトリを作成
                        Directory.CreateDirectory(Shared.Tool.ClassPath.DirectoryDelimiter(department.RootDirectory) + name);
                    }

                    // ディレクトリを作成
                    Directory.CreateDirectory(path);

                    // 機種の情報クラス
                    var information = new ClassDirectory();

                    // コメントを付加
                    information.Comment = textBoxComment.Text;

                    // 情報のファイルを書き込み
                    ClassManager.SaveDirectory(information, path);

                    // 戻り値を更新
                    this.DialogResult = true;

                    // 画面を閉じる
                    this.Close();
                }
                catch (Exception ex)
                {
                    // エラーのメッセージを表示
                    Shared.Tool.ClassException.Message(ex);
                }
            }
        }
    }
}
