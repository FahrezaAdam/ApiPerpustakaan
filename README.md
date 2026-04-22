# REST API - Sistem Manajemen Perpustakaan

## A. Deskripsi Project
Proyek ini adalah sebuah REST API untuk mengelola sistem informasi perpustakaan yang mencakup pendataan buku dan sistem autentikasi pengguna. Proyek ini dikembangkan untuk memenuhi LKM 1 mata kuliah Pemrograman Antarmuka Aplikasi. Domain ini dipilih karena merepresentasikan sistem transaksional di dunia nyata yang membutuhkan manajemen relasi data dan keamanan *endpoint* yang baik.

## B. Teknologi yang Digunakan
* **Bahasa Pemrograman:** C#
* **Framework:** ASP.NET Core 8.0 Web API
* **Database:** PostgreSQL
* **Library (NuGet Packages):**
  * `Npgsql`: Driver data untuk koneksi PostgreSQL.
  * `Dapper`: Micro-ORM untuk eksekusi query SQL yang aman dari *SQL Injection*.
  * `BCrypt.Net-Next`: Untuk pengamanan *hashing* password pengguna.
  * `Microsoft.AspNetCore.Authentication.JwtBearer`: Untuk sistem autentikasi berbasis Token JWT.

## C. Struktur Folder
Proyek ini menerapkan prinsip *Separation of Concerns* (SoC) dan *Dependency Injection* dengan struktur folder sebagai berikut:
* `Controllers/`: Berisi pengontrol *endpoint* API (`BukuController`, `AuthController`).
* `Models/`: Berisi representasi entitas database dan DTO (*Data Transfer Object*).
* `Helpers/`: Berisi kelas fungsionalitas bantuan, seperti `SqlDbHelper.cs` yang memusatkan logika koneksi ke database.
* `database.sql`: Skrip DDL dan DML untuk inisialisasi tabel dan data awal.

## D. Langkah Instalasi & Cara Menjalankan
1. Lakukan *Clone* repository ini ke direktori lokal komputer Anda.
2. Buka file *solution* (`.sln`) menggunakan **Visual Studio 2022**.
3. Pastikan *service* PostgreSQL lokal Anda dalam keadaan aktif.
4. Buka file `appsettings.json` dan sesuaikan *ConnectionStrings* (Host, Username, Password) agar sesuai dengan database PostgreSQL lokal Anda.
5. Tekan tombol **F5** atau klik ikon **Run** (https) di Visual Studio untuk menjalankan *server* lokal.
6. Halaman antarmuka **Swagger UI** akan otomatis terbuka di *browser* Anda.

## E. Cara Import Database
1. Buka aplikasi manajemen PostgreSQL (seperti pgAdmin).
2. Buat database baru dengan nama `db_perpustakaan`.
3. Buka tab *Query / SQL Editor*, lalu salin (*copy*) seluruh isi dari file `database.sql` yang ada di root repository ini.
4. Jalankan (*execute*) skrip tersebut. Tabel, relasi, dan minimal 5 baris *sample data* akan otomatis terbuat.

## F. Daftar Endpoint

### 1. Autentikasi (Publik)
* **POST `/api/auth/register`** Mendaftarkan akun pustakawan baru (Password otomatis di-hash).
* **POST `/api/auth/login`** Login akun untuk mendapatkan Token JWT.

### 2. Manajemen Buku (Wajib Login / Terotorisasi)
* **GET `/api/buku`** Mengambil semua daftar buku yang tersedia.
* **GET `/api/buku/{id}`** Mengambil detail informasi satu buku spesifik.
* **POST `/api/buku`** Menambahkan data buku baru ke dalam sistem.
* **PUT `/api/buku/{id}`** Mengubah atau memperbarui informasi buku.
* **DELETE `/api/buku/{id}`** Menghapus buku menggunakan metode *Soft Delete*.

## G. Cara Pengujian via Swagger
Karena sistem buku ini dilindungi oleh otorisasi, Anda harus melakukan *login* terlebih dahulu:
1. Buka *endpoint* `POST /api/auth/register` dan buat akun baru (contoh username: `admin`, password: `admin123`).
2. Buka *endpoint* `POST /api/auth/login`, masukkan kredensial yang sama.
3. Sistem akan merespons dengan kode **200 OK** dan memberikan teks `Token` yang panjang. Salin token tersebut.
4. Di bagian kanan atas halaman Swagger, klik tombol **Authorize** (ikon gembok).
5. Ketik `Bearer` diikuti dengan spasi, lalu *paste* token Anda (Contoh: `Bearer eyJhbGci...`).
6. Klik **Authorize** lalu **Close**. Sekarang Anda memiliki akses untuk menggunakan seluruh *endpoint* buku (CRUD).

## H. Link Video Presentasi
https://youtu.be/R29HmDW1qWc
