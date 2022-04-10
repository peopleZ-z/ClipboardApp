using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

// The main method is multithreaded by default. Single thread is required for the Clipboard class
Thread thread = new Thread(DoClipboard);
thread.SetApartmentState(ApartmentState.STA);
thread.Start();
thread.Join();

void DoClipboard()
{
    if (Clipboard.ContainsImage())
    {
        // A random name for the generated file
        string fileName = Guid.NewGuid().ToString();

        // Format is required
        string fileFormat = ".png";

        // A method. Current directory + filename -> to path string
        var path = () => Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}PNGs", $"{fileName}{fileFormat}");

        // Create subdirectory if not exist
        Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}PNGs");

        // If a file with such name already in the folder, generate new name
        while (File.Exists(path()))
        {
            fileName = Guid.NewGuid().ToString();
        }

        // Getting image from the windows clipboard
        BitmapSource image = Clipboard.GetImage();

        // Creating the file
        using (var fileStream = new FileStream(path(), FileMode.Create))
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(fileStream);
        }
    }

}