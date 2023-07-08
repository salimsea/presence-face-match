CREATE TABLE tb_pegawai (id_pegawai SERIAL NOT NULL, nama varchar(100) NOT NULL, nip varchar(16) NOT NULL UNIQUE, foto varchar(255) NOT NULL, created_by int4 NOT NULL, PRIMARY KEY (id_pegawai));
CREATE TABLE tb_pengaturan (id_pengaturan SERIAL NOT NULL, waktu_hadir time NOT NULL, waktu_keluar time NOT NULL, hightlight text NOT NULL, toleransi_waktu_hadir int4 NOT NULL, toleransi_waktu_keluar int4 NOT NULL, PRIMARY KEY (id_pengaturan));
CREATE TABLE tb_presensi (id_presensi SERIAL NOT NULL, id_pegawai int4 NOT NULL, jam_hadir time NOT NULL, jam_keluar time NOT NULL, tanggal timestamp NOT NULL, foto_hadir varchar(100) NOT NULL, foto_keluar varchar(100), waktu_hadir time NOT NULL, waktu_keluar time NOT NULL, PRIMARY KEY (id_presensi));
CREATE TABLE tb_user (id_user SERIAL NOT NULL, email varchar(100) NOT NULL UNIQUE, password varchar(255) NOT NULL, nama varchar(100) NOT NULL, PRIMARY KEY (id_user));
CREATE INDEX tb_pegawai_id_pegawai ON tb_pegawai (id_pegawai);
CREATE INDEX tb_pengaturan_id_pengaturan ON tb_pengaturan (id_pengaturan);
CREATE INDEX tb_presensi_id_presensi ON tb_presensi (id_presensi);
CREATE INDEX tb_user_id_user ON tb_user (id_user);
ALTER TABLE tb_presensi ADD CONSTRAINT "FK id_pegawai = id_pegawai" FOREIGN KEY (id_pegawai) REFERENCES tb_pegawai (id_pegawai);
ALTER TABLE tb_pegawai ADD CONSTRAINT "FK id_user = created_by" FOREIGN KEY (created_by) REFERENCES tb_user (id_user);

