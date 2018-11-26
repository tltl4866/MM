using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Media.Playback;
using Windows.UI.Xaml.Navigation;
using Senior_Project_V1.Music;
using System.Collections.ObjectModel;

namespace Senior_Project_V1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MusicApp : Page
    {
        private ObservableCollection<Sound> sounds;
        private List<MenuItem> MenuItems;
        private List<String> Suggestions;
        private Sound publicSound;
        public MusicApp()
        {
            this.InitializeComponent();
            sounds = new ObservableCollection<Sound>();
            SoundManager.GetAllSounds(sounds);
            MenuItems = new List<MenuItem>();
            MenuItems.Add(new MenuItem { IconFile = "Assets/Icon/note.png", Category = SoundCategory.Avicii });
            MenuItems.Add(new MenuItem { IconFile = "Assets/Icon/note2.png", Category = SoundCategory.Others });
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            goBack();
        }

        private void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(sender.Text)) goBack();

            SoundManager.GetAllSounds(sounds);
            Suggestions = sounds
                .Where(p => p.Name.StartsWith(sender.Text))
                .Select(p => p.Name)
                .ToList();
            SearchAutoSuggestBox.ItemsSource = Suggestions;
        }
        private void goBack()
        {
            SoundManager.GetAllSounds(sounds);
            CategoryTextBlock.Text = "All Sounds";
            MenuItemsListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            SoundManager.GetSoundsByName(sounds, sender.Text);
            CategoryTextBlock.Text = sender.Text;
            MenuItemsListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Visible;
        }


        private void MenuItemsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = (MenuItem)e.ClickedItem;

            // Filter on category
            CategoryTextBlock.Text = menuItem.Category.ToString();
            SoundManager.GetSoundsByCategory(sounds, menuItem.Category);
            BackButton.Visibility = Visibility.Visible;
        }


        public void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            publicSound = (Sound)e.ClickedItem;
            MyMediaElement.Source = new Uri(this.BaseUri, publicSound.AudioFile);
        }

        private void Rewind(object sender, RoutedEventArgs e)
        {
            DateTime date1 = new DateTime(2010, 8, 18, 13, 0, 0);
            DateTime date2 = new DateTime(2010, 8, 18, 13, 0, 10);
            TimeSpan interval = date2 - date1;
            MyMediaElement.Position = MyMediaElement.Position - interval;
        }

        private void Forward(object sender, RoutedEventArgs e)
        {
            MyMediaElement.PlaybackRate = 2.0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.DefaultPlaybackRate = 1.0;
            MyMediaElement.Play();
            Sound playing = null;
            var autoplay = true;
            while(autoplay)
            {
                if(MyMediaElement.Position == MyMediaElement.NaturalDuration)
                {
                    for (int i = 0; i < sounds.Count; i++)
                    {
                        playing = sounds[i];
                        if (playing == publicSound)
                        {
                            if (i + 1 == sounds.Count)
                            {
                                playing = sounds[0];
                                publicSound = playing;
                            }
                            else
                            {
                                playing = sounds[i + 1];
                                publicSound = playing;
                            }
                            break;
                        }
                    }
                    MyMediaElement.Source = new Uri(this.BaseUri, playing.AudioFile);
                    MyMediaElement.Play();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Pause();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Stop();
        }

        private void Repeat1(object sender, RoutedEventArgs e)
        {
            if (MyMediaElement.IsLooping == true)
                MyMediaElement.IsLooping = false;
            else
                MyMediaElement.IsLooping = true;
        }

        private void Mute(object sender, RoutedEventArgs e)
        {
            MyMediaElement.IsMuted = !MyMediaElement.IsMuted;
        }

        private void VolumeDown(object sender, RoutedEventArgs e)
        {
            if (MyMediaElement.IsMuted)
                MyMediaElement.IsMuted = false;
            if (MyMediaElement.Volume <= 1)
                MyMediaElement.Volume -= 0.1;
        }

        private void VolumeUp(object sender, RoutedEventArgs e)
        {
            if (MyMediaElement.IsMuted)
                MyMediaElement.IsMuted = false;
            if (MyMediaElement.Volume >= 0)
                MyMediaElement.Volume += 0.1;
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            Sound playing = null;
            for (int i = 0; i < sounds.Count; i++)
            {
                playing = sounds[i];
                if (playing == publicSound)
                {
                    if (i + 1 == sounds.Count)
                    {
                        playing = sounds[0];
                        publicSound = playing;
                    }
                    else
                    {
                        playing = sounds[i + 1];
                        publicSound = playing;
                    }
                    break;
                }
            }
            MyMediaElement.Source = new Uri(this.BaseUri, playing.AudioFile);
            MyMediaElement.Play();
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            Sound playing = null;
            for (int i = 0; i < sounds.Count; i++)
            {
                playing = sounds[i];
                if (playing == publicSound)
                {
                    if (i - 1  < 0)
                    {
                        playing = sounds[sounds.Count - 1];
                        publicSound = playing;
                    }
                    else
                    {
                        playing = sounds[i - 1];
                        publicSound = playing;
                    }
                    break;
                }
            }
            MyMediaElement.Source = new Uri(this.BaseUri, playing.AudioFile);
            MyMediaElement.Play();
        }

        private void SoundGridView_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "drop to create a custom sound and title";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private async void SoundGridView_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();

                if (items.Any())
                {
                    var storageFile = items[0] as StorageFile;
                    var contentType = storageFile.ContentType;

                    StorageFolder folder = ApplicationData.Current.LocalFolder;

                    if (contentType == "audio/wav" || contentType == "audio/mp3")
                    {
                        StorageFile newFile = await storageFile.CopyAsync(folder, storageFile.Name, NameCollisionOption.GenerateUniqueName);

                        MyMediaElement.SetSource(await storageFile.OpenAsync(FileAccessMode.Read), contentType);
                        MyMediaElement.Play();
                    }
                }
            }
        }

        private void HomeButtom_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WelcomePage));
            MyMediaElement.AutoPlay = true;
        }

        private void CurrentState_Changed(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = sender as MediaElement;
            if (mediaElement != null && mediaElement.IsAudioOnly == false)
            {
                if (mediaElement.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing)
                {
                    MyMediaElement.Play();
                }
                else // CurrentState is Buffering, Closed, Opening, Paused, or Stopped. 
                {
                    MyMediaElement.Play();
                }
            }
        }
    }
}
