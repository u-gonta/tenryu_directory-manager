using DirectoryManagerTsp.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        /// <summary>
        /// 情報のデータグリッド用クラス
        /// </summary>
        public class ClassDataGridInformation : INotifyPropertyChanged
        {
            /// <summary>
            /// プロパティの変更イベント
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// 名前
            /// </summary>
            private string _name;

            /// <summary>
            /// 生成する種類
            /// </summary>
            private Shared.Generation.EnumCategory _generation;

            /// <summary>
            /// 名前のプロパティ
            /// </summary>
            public string IsName
            {
                get
                {
                    return _name;
                }

                set
                {
                    _name = value;
                }
            }

            /// <summary>
            /// 生成する種類のプロパティ
            /// </summary>
            public Shared.Generation.EnumCategory IsGeneration
            {
                get
                {
                    return _generation;
                }

                set
                {
                    _generation = value;
                }
            }

            /// <summary>
            /// 無効のプロパティ
            /// </summary>
            public bool IsInvalid
            {
                get
                {
                    bool ret = false;

                    if (_generation == Shared.Generation.EnumCategory.Invalid)
                    {
                        ret = true;
                    }

                    return ret;
                }

                set
                {
                    _generation = Shared.Generation.EnumCategory.Invalid;

                    // プロパティの変更を呼び出し
                    CallPropertyChanged("IsEmpty");
                    CallPropertyChanged("IsCopy");
                    CallPropertyChanged("IsLink");
                }
            }

            /// <summary>
            /// 空のプロパティ
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    bool ret = false;

                    if (_generation == Shared.Generation.EnumCategory.Empty)
                    {
                        ret = true;
                    }

                    return ret;
                }

                set
                {
                    _generation = Shared.Generation.EnumCategory.Empty;

                    // プロパティの変更を呼び出し
                    CallPropertyChanged("IsInvalid");
                    CallPropertyChanged("IsCopy");
                    CallPropertyChanged("IsLink");
                }
            }

            /// <summary>
            /// コピーのプロパティ
            /// </summary>
            public bool IsCopy
            {
                get
                {
                    bool ret = false;

                    if (_generation == Shared.Generation.EnumCategory.Copy)
                    {
                        ret = true;
                    }

                    return ret;
                }

                set
                {
                    _generation = Shared.Generation.EnumCategory.Copy;

                    // プロパティの変更を呼び出し
                    CallPropertyChanged("IsEmpty");
                    CallPropertyChanged("IsInvalid");
                    CallPropertyChanged("IsLink");
                }
            }

            /// <summary>
            /// リンクのプロパティ
            /// </summary>
            public bool IsLink
            {
                get
                {
                    bool ret = false;

                    if (_generation == Shared.Generation.EnumCategory.Link)
                    {
                        ret = true;
                    }

                    return ret;
                }

                set
                {
                    _generation = Shared.Generation.EnumCategory.Link;

                    // プロパティの変更を呼び出し
                    CallPropertyChanged("IsInvalid");
                    CallPropertyChanged("IsEmpty");
                    CallPropertyChanged("IsCopy");
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassDataGridInformation()
            {
                this._name = "";
                this._generation = Shared.Generation.EnumCategory.Invalid;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassDataGridInformation(string name, Shared.Generation.EnumCategory generation) : base()
            {
                this._name = name;
                this._generation = generation;
            }

            /// <summary>
            /// 情報の項目クラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassDataGridInformation Clone()
            {
                ClassDataGridInformation ret = new ClassDataGridInformation();

                ret = (ClassDataGridInformation)MemberwiseClone();

                return ret;
            }

            /// <summary>
            /// プロパティの変更を呼び出し
            /// </summary>
            /// <param name="name">プロパティの名称</param>
            private void CallPropertyChanged(string name = null)
            {
                do
                {
                    // プロパティの変更を確認
                    if (PropertyChanged == null)
                    {
                        // プロパティの変更がない
                        break;
                    }
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
                } while (false);
            }
        }

        /// <summary>
        /// 部署のコンボボックス用クラス
        /// </summary>
        public class ClassComboboxItemDepartment
        {
            /// <summary>
            /// 部署のクラス
            /// </summary>
            public Shared.Setting.ClassDepartment Department { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Title
            {
                get
                {
                    string ret = "";

                    ret = Department.Title;

                    return ret;
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassComboboxItemDepartment()
            {
                this.Department = new Shared.Setting.ClassDepartment();
                this.Department.Title = Shared.Select.ClassDefine.UnselectedName;
            }

            /// <summary>
            /// コンボボックスの項目クラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassComboboxItemDepartment Clone()
            {
                var ret = new ClassComboboxItemDepartment();

                ret = (ClassComboboxItemDepartment)MemberwiseClone();

                // 部署のクラスをコピー
                ret.Department = this.Department.Clone();

                return ret;
            }
        }

        /// <summary>
        /// WindowInformation.xaml の相互作用ロジック
        /// </summary>
        public partial class WindowInformation : Window
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
            /// 部署コンボボックスの項目クラス
            /// </summary>
            private ObservableCollection<ClassComboboxItemDepartment> _departmentComboboxItems = new ObservableCollection<ClassComboboxItemDepartment>();

            /// <summary>
            /// 選択している部署のパス
            /// </summary>
            private string _departmentSelected = "";

            /// <summary>
            /// ディレクトリ情報のデータグリッドの項目クラス
            /// </summary>
            private ObservableCollection<ClassDataGridInformation> _dataGridDirectory = new ObservableCollection<ClassDataGridInformation>();

            /// <summary>
            /// ファイル情報のデータグリッドの項目クラス
            /// </summary>
            private ObservableCollection<ClassDataGridInformation> _dataGridFile = new ObservableCollection<ClassDataGridInformation>();

            /// <summary>
            /// 操作中のディレクトリ
            /// </summary>
            private string _path = "";

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="model">機種名</param>
            /// <param name="equipment" >装置名</param>
            public WindowInformation(string model, string equipment)
            {
                InitializeComponent();

                try
                {
                    // 機種名を更新
                    _model = model;

                    // 装置名を更新
                    _equipment = equipment;

                    // ディレクトリ選択時の通知を登録
                    controlDirectoryTree.DirectorySelected += this.OnSelected;

                    // 部署名のコンボボックスを更新
                    UpdateDepartmentCombobox();
                }
                catch (Exception ex)
                {
                    // エラーのメッセージを表示
                    Shared.Tool.ClassException.Message(ex);
                }
            }

            /// <summary>
            /// 部署名のコンボボックスを選択
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void comboboxDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    do
                    {
                        // 選択しているコンボボックスの項目を取得
                        var comboboxItem = ((ComboBox)sender).SelectedItem as ClassComboboxItemDepartment;

                        // コンボボックスの項目を確認
                        if (comboboxItem == null)
                        {
                            // コンボボックスの項目が未選択
                            break;
                        }

                        // コンボボックスを選択中の項目
                        string selectedDirectory = comboboxItem.Department.RootDirectory;

                        // 選択しているパスを確認
                        if (_departmentSelected != selectedDirectory)
                        {
                            // 部署名が変更 ⇒ 選択しているの部署のパスを更新
                            _departmentSelected = selectedDirectory;

                            do
                            {
                                // 部署のディレクトリを取得
                                var directory = selectedDirectory;

                                // ディレクトリの区切りを付加
                                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                                // 機種名を付加
                                directory += _model;

                                // ディレクトリの区切りを付加
                                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                                // 装置名を付加
                                directory += _equipment;

                                // ディレクトリのツリーを初期化
                                controlDirectoryTree.Update(new DirectoryInfo(directory));
                            } while (false);
                        }
                    } while (false);
                }
                catch (Exception ex)
                {
                    // エラーのログを出力
                    Shared.Tool.ClassException.Logging(ex);
                }
            }

            /// <summary>
            /// ディレクトリの選択時に呼び出される関数
            /// </summary>
            /// <param name="eventArgs">ディレクトリの選択時に渡されるクラス</param>
            private void OnSelected(Shared.Tool.DirectoryTreeEventArgs args)
            {
                try
                {
                    // ディレクトリのパスを更新
                    _path = args.Path;

                    // ディレクトリ情報の項目クラスを初期化
                    _dataGridDirectory = new ObservableCollection<ClassDataGridInformation>();
                    // ファイル情報の項目クラスを初期化
                    _dataGridFile = new ObservableCollection<ClassDataGridInformation>();

                    do
                    {
                        // ディレクトリを確認
                        if (Directory.Exists(_path) == false)
                        {
                            // ディレクトリなし ⇒ ディレクトリ内を探索する処理を抜ける
                            break;
                        }

                        // ディレクトリ内のディレクトリを探索
                        IEnumerable<string> directories = Directory.EnumerateDirectories(_path, "*");
                        // ディレクトリ内のファイルを探索
                        IEnumerable<string> files = Directory.EnumerateFiles(_path, "*");

                        // 情報のファイル読み込み
                        var information = ClassManager.LoadDirectory(_path);

                        // ディレクトリを走査
                        foreach (var directory in directories)
                        {
                            var generation = Shared.Generation.EnumCategory.Invalid;

                            // ディレクトリ名へ分割
                            var name = Path.GetFileName(directory);

                            // ディレクトリ名を確認
                            if (information.Directories.ContainsKey(name))
                            {
                                // データあり
                                generation = information.Directories[name].Category;
                            }

                            // ディレクトリの情報を追加
                            _dataGridDirectory.Add(new ClassDataGridInformation(name, generation));
                        }

                        // ファイルを走査
                        foreach (var file in files)
                        {
                            // ファイル名へ分割
                            var name = Path.GetFileName(file);

                            // 情報のファイルか確認
                            if (name == ClassManager.DirectoryInformationName)
                            {
                                // 情報のファイル
                                continue;
                            }

                            var generation = Shared.Generation.EnumCategory.Invalid;

                            // ディレクトリ名を確認
                            if (information.Files.ContainsKey(name))
                            {
                                // データあり
                                generation = information.Files[name].Category;
                            }

                            // ファイルの情報を追加
                            _dataGridFile.Add(new ClassDataGridInformation(name, generation));
                        }
                    } while (false);

                    // データグリッド(ディレクトリ用)の項目を更新
                    dataGridDirectory.ItemsSource = _dataGridDirectory;

                    // データグリッド(ディレクトリ用)の選択を解除
                    dataGridDirectory.SelectedIndex = 0;

                    // データグリッド(ファイル用)の項目を更新
                    dataGridFile.ItemsSource = _dataGridFile;

                    // データグリッド(ファイル用)の選択を解除
                    dataGridFile.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    // エラーのログを出力
                    Shared.Tool.ClassException.Logging(ex);
                }
            }

            /// <summary>
            /// 保存ボタンのクリック
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void buttonSave_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    do
                    {
                        // パスを確認
                        if (Directory.Exists(_path) == false)
                        {
                            // パスが未選択
                            break;
                        }

                        // 情報のファイル読み込み
                        var directoryInformation = ClassManager.LoadDirectory(_path);

                        // ディレクトリの情報を走査
                        foreach (var directory in _dataGridDirectory)
                        {
                            // 登録してある名前を確認
                            var valid = directoryInformation.Directories.ContainsKey(directory.IsName);

                            // 生成する種類を確認
                            if (directory.IsInvalid)
                            {
                                // 無効 ⇒ 登録してある名前を確認
                                if (valid)
                                {
                                    // 登録あり ⇒ 登録を解除
                                    directoryInformation.Directories.Remove(directory.IsName);
                                }
                                continue;
                            }

                            var information = new Shared.Generation.ClassInformation();

                            // 登録してある名前を確認
                            if (valid)
                            {
                                // 登録あり ⇒ 登録してあるデータをコピー
                                information = directoryInformation.Directories[directory.IsName];
                            }

                            // 生成する種類を更新
                            information.Category = directory.IsGeneration;

                            // 情報を更新
                            directoryInformation.Directories[directory.IsName] = information;
                        }

                        // ファイルの情報を走査
                        foreach (var file in _dataGridFile)
                        {
                            // 登録してある名前を確認
                            var valid = directoryInformation.Files.ContainsKey(file.IsName);

                            // 生成する種類を確認
                            if (file.IsInvalid)
                            {
                                // 無効 ⇒ 登録してある名前を確認
                                if (valid)
                                {
                                    // 登録あり ⇒ 登録を解除
                                    directoryInformation.Files.Remove(file.IsName);
                                }
                                continue;
                            }

                            var information = new Shared.Generation.ClassInformation();

                            // 登録してある名前を確認
                            if (valid)
                            {
                                // 登録あり ⇒ 登録してあるデータをコピー
                                information = directoryInformation.Files[file.IsName];
                            }

                            // 生成する種類を更新
                            information.Category = file.IsGeneration;

                            // 情報を更新
                            directoryInformation.Files[file.IsName] = information;
                        }

                        // 上方のファイルを書き込み
                        ClassManager.SaveDirectory(directoryInformation, _path);
                    } while (false);
                }
                catch (Exception ex)
                {
                    // エラーのメッセージを表示
                    Shared.Tool.ClassException.Message(ex);
                }
            }

            /// <summary>
            /// 部署名のコンボボックスを更新
            /// </summary>
            private void UpdateDepartmentCombobox()
            {
                // コンボボックスの項目クラスを初期化
                _departmentComboboxItems = new ObservableCollection<ClassComboboxItemDepartment>();

                // 未選択のコンボボックスの項目を追加
                _departmentComboboxItems.Add(new ClassComboboxItemDepartment());

                var settingManager = Setting.ClassIntegrationManager.Instance;
                var integration = settingManager.GetIntegration();
                int index = 0;

                // 部署名を走査
                foreach (var department in integration.Departments)
                {
                    var comboboxItem = new ClassComboboxItemDepartment();

                    comboboxItem.Department = department.Clone();

                    // 部署名を確認
                    if (_departmentSelected == comboboxItem.Department.RootDirectory)
                    {
                        // 選択中の部署名と一致
                        index = _departmentComboboxItems.Count;
                    }

                    // コンボボックスの項目を追加
                    _departmentComboboxItems.Add(comboboxItem);
                }

                // コンボボックスの項目を更新
                comboboxDepartment.ItemsSource = _departmentComboboxItems;

                // コンボボックスを選択
                comboboxDepartment.SelectedIndex = index;
            }
        }
    }
}
