using System;
using System.Windows;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        namespace Equipment
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
                /// コンストラクタ
                /// </summary>
                /// <param name="model">機種名</param>
                /// <param name="equipment">装置名</param>
                public WindowComment(string model, string equipment)
                {
                    InitializeComponent();

                    try
                    {
                        // 機種名を更新
                        _model = model;

                        // 装置名を更新
                        _equipment = equipment;

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
                        // 装置ディレクトリを取得
                        var path = GetPath();

                        // 装置ディレクトリにある情報のファイル読み込み
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

                    // 機種ディレクトリを取得
                    var path = GetModelPath();

                    // 参照する機種ディレクトリにある情報のファイル読み込み
                    var information = ClassManager.LoadDirectory(path);

                    // 機種のコメントを更新
                    labelModelComment.Content = information.Comment;

                    // 装置ディレクトリを取得
                    path = GetPath();

                    // 参照する装置ディレクトリにある情報のファイル読み込み
                    information = ClassManager.LoadDirectory(path);

                    // 派生元の装置名を確認
                    if (0 < information.DerivationEquipment.Length)
                    {
                        // 派生元の装置名がある ⇒ 派生元の装置名を付加
                        labelDerivationEquipment.Content = "派生元：" + information.DerivationEquipment;
                    }

                    // コメントを更新
                    textBoxComment.Text = information.Comment;
                }

                /// <summary>
                /// 機種ディレクトリを取得
                /// </summary>
                /// <returns>機種ディレクトリ</returns>
                private string GetModelPath()
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

                    return ret;
                }

                /// <summary>
                /// 装置ディレクトリを取得
                /// </summary>
                /// <returns>装置ディレクトリ</returns>
                private string GetPath()
                {
                    string ret = "";

                    // 機種ディレクトリを取得
                    ret = GetModelPath();

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 装置名を付加
                    ret += _equipment;

                    return ret;
                }
            }
        }
    }
}
