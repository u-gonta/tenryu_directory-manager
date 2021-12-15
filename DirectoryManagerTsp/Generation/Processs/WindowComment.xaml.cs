using System;
using System.Windows;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        namespace Process
        {
            /// <summary>
            /// WindowComment.xaml の相互作用ロジック
            /// </summary>
            public partial class WindowComment : Window
            {
                /// <summary>
                /// 機種名
                /// </summary>
                private string _model = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// 装置名
                /// </summary>
                private string _equipment = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// 工番名
                /// </summary>
                private string _process = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="model">機種名</param>
                /// <param name="equipment">装置名</param>
                /// <param name="process">工番名</param>
                public WindowComment(string model, string equipment, string process)
                {
                    InitializeComponent();

                    try
                    {
                        // 機種名を更新
                        _model = model;

                        // 装置名を更新
                        _equipment = equipment;

                        // 工番名を更新
                        _process = process;

                        // 画面を初期化
                        InitializeWindow();
                    }
                    catch (Exception ex)
                    {
                        // エラーのメッセージを表示
                        Shared.Tool.ClassException.Message(ex);
                    }
                }

                /// <summary>
                /// 更新ボタンクリック
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void buttonUpdate_Click(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        // 参照する装置ディレクトリを取得
                        var path = GetPath();

                        // 参照する装置ディレクトリにある情報のファイル読み込み
                        var information = ClassManager.LoadDirectory(path);

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

                /// <summary>
                /// 画面を初期化
                /// </summary>
                private void InitializeWindow()
                {
                    // 機種名を更新
                    labelModel.Content = _model;

                    // 装置名を更新
                    labelEquipment.Content = _equipment;

                    // 工番名を更新
                    labelProcess.Content = _process;

                    var path = GetPath();

                    // 参照する装置ディレクトリにある情報のファイル読み込み
                    var informationSource = ClassManager.LoadDirectory(path);

                    // コメントを更新
                    textBoxComment.Text = informationSource.Comment;
                }

                /// <summary>
                /// ディレクトリのパスを取得
                /// </summary>
                /// <returns>ディレクトリのパス</returns>
                private string GetPath()
                {
                    string ret = "";

                    var settingManager = Setting.ClassIntegrationManager.Instance;
                    var integration = settingManager.GetIntegration();

                    // テンプレートのルートディレクトリ
                    ret = integration.TemplateRootDirectory;

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 機種名を付加
                    ret += _model;

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 装置名を付加
                    ret += _equipment;

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 工番のディレクトリ名を付加
                    ret += integration.TemplateProcessDirectory;

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 工番名を付加
                    ret += _process;

                    return ret;
                }
            }
        }
    }
}
