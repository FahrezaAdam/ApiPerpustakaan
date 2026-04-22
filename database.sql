-- DDL: Tabel 1 - buku (Tabel Master dengan Soft Delete)
CREATE TABLE buku (
    id_buku SERIAL PRIMARY KEY,
    judul VARCHAR(150) NOT NULL,
    penulis VARCHAR(100) NOT NULL,
    tahun_terbit INT NOT NULL,
    dibuat_pada TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    diperbarui_pada TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    dihapus_pada TIMESTAMP NULL DEFAULT NULL
);
CREATE INDEX idx_judul_buku ON buku(judul);

-- DDL: Tabel 2 - anggota
CREATE TABLE anggota (
    id_anggota SERIAL PRIMARY KEY,
    nama_lengkap VARCHAR(100) NOT NULL,
    nomor_telepon VARCHAR(20) NOT NULL,
    dibuat_pada TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    diperbarui_pada TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- DDL: Tabel 3 - peminjaman
CREATE TABLE peminjaman (
    id_peminjaman SERIAL PRIMARY KEY,
    id_buku INT NOT NULL,
    id_anggota INT NOT NULL,
    tanggal_pinjam DATE NOT NULL,
    tanggal_kembali DATE NULL,
    status_pinjam VARCHAR(20) DEFAULT 'Dipinjam' CHECK (status_pinjam IN ('Dipinjam', 'Dikembalikan')),
    dibuat_pada TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    diperbarui_pada TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_buku) REFERENCES buku(id_buku) ON DELETE CASCADE,
    FOREIGN KEY (id_anggota) REFERENCES anggota(id_anggota) ON DELETE CASCADE
);
CREATE INDEX idx_status_pinjam ON peminjaman(status_pinjam);

-- DDL: Tabel 3 - pengguna
CREATE TABLE pengguna (
    id_pengguna SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    peran VARCHAR(20) DEFAULT 'Pustakawan',
    dibuat_pada TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- DML: Insert Sample Data (Minimal 5 baris)
INSERT INTO buku (judul, penulis, tahun_terbit) VALUES
('Pemrograman C# untuk Pemula', 'Budi Raharjo', 2021),
('Clean Code', 'Robert C. Martin', 2008),
('Struktur Data dan Algoritma', 'Ema Utami', 2019),
('Desain Antarmuka Pengguna', 'Andi Taru', 2020),
('Mastering ASP.NET Core', 'Christian Nagel', 2022);

INSERT INTO anggota (nama_lengkap, nomor_telepon) VALUES
('Ahmad Fauzi', '081234567890'),
('Siti Aminah', '081987654321'),
('Rudi Hermawan', '081555666777'),
('Dewi Lestari', '081122334455'),
('Gilang Ramadhan', '081999888777');

INSERT INTO peminjaman (id_buku, id_anggota, tanggal_pinjam, tanggal_kembali, status_pinjam) VALUES
(1, 1, '2023-10-01', '2023-10-08', 'Dikembalikan'),
(2, 2, '2023-10-02', NULL, 'Dipinjam'),
(3, 3, '2023-10-05', '2023-10-12', 'Dikembalikan'),
(4, 4, '2023-10-10', NULL, 'Dipinjam'),
(5, 5, '2023-10-15', NULL, 'Dipinjam');
