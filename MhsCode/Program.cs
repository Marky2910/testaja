/*
    Nama: Mark Siregar
    NIM: 2281071
*/

using System;
using System.IO;

struct Mahasiswa{
    public int NIM;
    public string Nama;
    public int[] Tugas;

    // Constructor to initialize the Struct
    public Mahasiswa(int nim, string nama, int[] tugas){
        NIM = nim;
        Nama = nama;
        Tugas = tugas;
    }
}

class Program{
    static string filePath = "data_mahasiswa.txt";
    static Mahasiswa[] mhsArray = new Mahasiswa[3]; // Initialize before use

    static void Main(){
    if(File.Exists(filePath)){
        LoadData(mhsArray);
    }

    // Menu options
    while (true) {
        Console.WriteLine("===========================================");
        Console.WriteLine("1. Add Data, 2. Edit/Delete Data, 3. Display Data, 0. Exit\nChoose an option:");
        
        string input = Console.ReadLine();
        int pilihan;
        
        // Validate the input using int.TryParse to avoid FormatException
        if (!int.TryParse(input, out pilihan)) {
            Console.WriteLine("ERROR: Please enter a valid number!");
            continue; // Prompt the user again
        }

        switch (pilihan){
            case 1: // Add new data
                Console.WriteLine("Masukkan jumlah Mahasiswa: ");
                int jumlah;
                while (!int.TryParse(Console.ReadLine(), out jumlah)) {
                    Console.WriteLine("ERROR: Please enter a valid number!");
                }
                InputDataTugas(mhsArray, jumlah);
                SaveData(mhsArray);
                break;

            case 2: // Edit/Delete data
                Console.WriteLine("NIM yang ingin Diubah/hapus: ");
                int nimX;
                while (!int.TryParse(Console.ReadLine(), out nimX)) {
                    Console.WriteLine("ERROR: Please enter a valid number!");
                }
                EditOrDelete(nimX);
                break;

            case 3: // Display data (Read functionality)
                DisplayData(mhsArray);
                break;

            case 0:
                Console.WriteLine("Terima Kasih sudah menggunakan aplikasi ini!");
                return;

            default:
                Console.WriteLine("Opsi tidak ada!");
                break;
        }
    }
}


    static void EditOrDelete(int nimX) {
        for (int i = 0; i < mhsArray.Length; i++) {
            if (mhsArray[i].NIM == nimX) {
                Console.WriteLine("NIM Found");
                Console.WriteLine("1. Edit, 2. Delete");
                Console.WriteLine("Masukkan Opsi: ");
                int opsi = int.Parse(Console.ReadLine());

                if (opsi == 1){ // Update
                    Console.WriteLine("Masukkan NIM Baru: ");
                    int nimNew = int.Parse(Console.ReadLine());
                    Console.WriteLine("Masukkan Nama baru: ");
                    string namaNew = Console.ReadLine();
                    int[] tugasNew = new int[3];
                    for (int j = 0; j < 3; j++){
                        Console.WriteLine($"Masukkan Tugas-{j + 1} Baru: ");
                        tugasNew[j] = int.Parse(Console.ReadLine());
                    }

                    mhsArray[i] = new Mahasiswa(nimNew, namaNew, tugasNew);
                    SaveData(mhsArray);

                } else if (opsi == 2){ // Delete
                    DeleteData(i);
                    SaveData(mhsArray);
                } else {
                    Console.WriteLine("Opsi Salah!");
                }
                return;
            }
        }
        Console.WriteLine("NIM tidak ditemukan!");
    }

    static void DeleteData(int index){
        for (int i = index; i < mhsArray.Length - 1; i++){
            mhsArray[i] = mhsArray[i + 1]; // Shift data left
        }
        Array.Resize(ref mhsArray, mhsArray.Length - 1); // Reduce array size
    }

    static void InputDataTugas(Mahasiswa[] mhsArray, int n){
        for (int i = 0; i < n; i++){
            Console.Clear();
            Console.WriteLine($"Masukan detail Mahasiswa ke-{i + 1}:");

            int nim = InputNIM();
            Console.Write("Nama: ");
            string nama = Console.ReadLine();

            int[] tugas = new int[3];
            for (int j = 0; j < 3; j++){
                Console.Write($"Tugas ke-{j + 1}: ");
                tugas[j] = int.Parse(Console.ReadLine());
            }

            mhsArray[i] = new Mahasiswa(nim, nama, tugas);
        }
    }

    static int InputNIM(){
        int nim;
        while (true){
            try {
                Console.Write("NIM: ");
                nim = int.Parse(Console.ReadLine());
                break;
            } catch (FormatException) {
                Console.WriteLine("ERROR : NIM should contain only numbers. Please reinput the NIM.");
            }
        }
        return nim;
    }

    static void CetakDataMahasiswa(Mahasiswa[] mhsArray, int i){
    // Ensure the element is initialized
    if (mhsArray[i].Nama != null){
        Console.Write($"{i + 1}.");
        Console.Write($"\t{mhsArray[i].NIM}");
        Console.Write($"\t{mhsArray[i].Nama}");
        for (int j = 0; j < mhsArray[i].Tugas.Length; j++){
            Console.Write($"\t{mhsArray[i].Tugas[j]}");
        }
        Console.WriteLine();
    } else {
        Console.WriteLine($"Data for Mahasiswa at index {i} is not initialized.");
    }
}


    // Read Functionality: Display all student data
    static void DisplayData(Mahasiswa[] mhsArray) {
    Console.WriteLine("===========================================");
    Console.WriteLine("No.\tNIM\tNama\tTugas-1\tTugas-2\tTugas-3");
    for (int i = 0; i < mhsArray.Length; i++) {
        // Only display initialized Mahasiswa entries
        if (mhsArray[i].Nama != null){
            CetakDataMahasiswa(mhsArray, i);
        }
    }
    Console.WriteLine("===========================================");
}


    static void SaveData(Mahasiswa[] mhsArray){
    using (StreamWriter writer = new StreamWriter(filePath)){
        foreach (var mhs in mhsArray){
            // Check if the Mahasiswa object is valid (NIM is not 0 and Nama is not null)
            if (mhs.NIM != 0 && !string.IsNullOrEmpty(mhs.Nama)){
                writer.WriteLine($"{mhs.NIM},{mhs.Nama},{string.Join(",", mhs.Tugas)}");
            }
        }
    }
}


    static void LoadData(Mahasiswa[] mhsArray){
        try {
            string[] baris = File.ReadAllLines(filePath);
            for (int i = 0; i < baris.Length && i < mhsArray.Length; i++){
                string[] data = baris[i].Split(',');
                int nim = int.Parse(data[0]);
                string nama = data[1];
                int[] tugas = Array.ConvertAll(data[2..], int.Parse);
                mhsArray[i] = new Mahasiswa(nim, nama, tugas); // Assign loaded data to array
            }
        } catch (Exception ex) {
            Console.WriteLine($"Error Loading Data: {ex.Message}");
        }
    }
}
