using LibVLCSharp;

namespace VlcBugFormResize;

public partial class VideoForm : Form
{
    private readonly MediaPlayer _mediaPlayer;
    private readonly LibVLC _libVlc;
    
    public VideoForm()
    {
        InitializeComponent();
        
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc)
        {
            Hwnd = videoPanel.Handle,
            EnableKeyInput = false,
            EnableMouseInput = false,
            EnableHardwareDecoding = true,
        };
    }

    private async void loadButton_Click(object sender, EventArgs e)
    {
        var fileDialog = new OpenFileDialog()
        {
            Filter = @"All files (*.*)|*.*",
            Title = @"Open video file"
        };

        if (fileDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            var filePath = fileDialog.FileName;
            using var media = new Media(_libVlc, filePath);
            _mediaPlayer.Media = media;
            
            // Load file
            await _mediaPlayer.Media.ParseAsync();
            _mediaPlayer.SetPosition(0);

            // Play
            await _mediaPlayer.PlayAsync();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    
    private static void ShowError(string message)
    {
        MessageBox.Show(message, @"Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}