import { useEffect, useState } from "react";
import { Layout } from "../components";
import { Button, Modal, Label, TextInput } from "flowbite-react";
import { instanceAuth } from "../helpers";
import Swal from "sweetalert2";
import { Controller, useForm } from "react-hook-form";
import Select from "react-select";
import { IMGNoPhoto } from "../assets";

const Laporan = () => {
  const [showModal, setShowModal] = useState(false);
  const [showModalFilter, setShowModalFilter] = useState(false);
  const [dataPresensis, setDataPresensis] = useState([]);
  const [dataPegawais, setDataPegawais] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [tanggalAwal, setTanggalAwal] = useState(null);
  const [tanggalAkhir, setTanggalAkhir] = useState(null);
  
  const {
    register,
    handleSubmit,
    reset,
    control,
    getValues,
    formState: { errors },
  } = useForm();
  const {
    register: registerFilter,
    handleSubmit: handleSubmitFilter,
    formState: { errors: errorsFilter },
  } = useForm();

  useEffect(() => {
    getPresensis();
    getPegawais();
  }, []);

  const getPresensis = (data = null) => {
    Swal.showLoading();
      var param = "";
    if (data) {
      var tglAwal = FUNCYmdToDmy(data.tanggalAwal),
        tglAkhir = FUNCYmdToDmy(data.tanggalAkhir);
      param = `tanggalAwal=${tglAwal}&tanggalAkhir=${tglAkhir}`;
      setTanggalAwal(tglAwal)
      setTanggalAkhir(tglAkhir)
    }
    instanceAuth.get(`/presensi/getPresensis?${param}`).then((res) => {
      var data = res.data;
      Swal.close()
      if (data.isSuccess) {
        setDataPresensis(data.data);
        setShowModalFilter(false);
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  const getPegawais = () => {
    instanceAuth.get("/presensi/getpegawais").then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        var pegawais = [];
        data.data.map((item) => {
          pegawais.push({
            value: item.idPegawai,
            label: item.nama,
          });
        });
        setDataPegawais(pegawais);
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  const FUNCYmdToDmy = (tgl) => {
    let tanggalArray = tgl.split("-");
    let tanggalArrayTerbalik = tanggalArray.reverse();
    return tanggalArrayTerbalik.join("-");
  };

  const btnAddPresensi = (data) => {
    var fd = new FormData();
    Swal.showLoading();
    fd.append("tanggal", FUNCYmdToDmy(data.tanggal));
    data.idPegawais.map((item, index) => {
      fd.append(`idPegawai[${index}]`, item.value);
    });
    instanceAuth.post("/presensi/checkinManual", fd).then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        Swal.fire(
          "Berhasil",
          "Anda berhasil menambahkan pegawai baru!",
          "success"
        );
        setShowModal(false);
        getPresensis();
        reset();
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  const btnDownload = () => {
      Swal.showLoading();
      var param = "";
    if (tanggalAwal)
      param = `tanggalAwal=${tanggalAwal}&tanggalAkhir=${tanggalAkhir}`;
     instanceAuth
      .get(
        `presensi/DownloadLaporanPresensi?${param}`,
        {
          headers: {
            "Content-Disposition": "attachment; filename=template.xlsx",
            "Content-Type":
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          },
          responseType: "arraybuffer",
        }
      )
       .then((response) => {
        Swal.fire(
          "Berhasil",
          "Laporan berhasil diunduh!",
          "success"
        );
      const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute(
          "download",
          `laporan-presensi-karyawan.xlsx`
        );
        document.body.appendChild(link);
        link.click();
      })
       .catch((error) => {
        Swal.fire("Gagal", "Galat error!", "error");
        
        console.log(error);
      });
  }

  return (
    <Layout>
      <div className="">
        <div className="sm:rounded-lg">
          <div className="flex items-center justify-between pb-4  dark:bg-gray-900">
            <div className="flex gap-3">
              <Button onClick={() => setShowModal(true)}>
                Presensi Manual
              </Button>
              <Button color="warning" onClick={() => btnDownload()}>
                Download
              </Button>
              <Button color="dark" onClick={() => setShowModalFilter(true)}>
                Filter Tanggal
              </Button>
            </div>
            <label htmlFor="table-search" className="sr-only">
              Search
            </label>
            <div className="relative">
              <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
                <svg
                  className="w-5 h-5 text-gray-500 dark:text-gray-400"
                  aria-hidden="true"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    fillRule="evenodd"
                    d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                    clipRule="evenodd"
                  ></path>
                </svg>
              </div>
              <input
                type="text"
                id="table-search-users"
                className="block p-2 pl-10 text-sm text-gray-900 border border-gray-300 rounded-lg w-80 bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                placeholder="Cari Pegawai"
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </div>
          </div>
          <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
            <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
              <tr>
                <th scope="col" className="px-6 py-3">
                  Tanggal
                </th>
                <th scope="col" className="px-6 py-3">
                  Nama
                </th>
                <th scope="col" className="px-6 py-3">
                  Jam Hadir
                </th>
                <th scope="col" className="px-6 py-3">
                  Jam Keluar
                </th>
                <th scope="col" className="px-6 py-3">
                  Foto Hadir
                </th>
                <th scope="col" className="px-6 py-3">
                  Foto Keluar
                </th>
              </tr>
            </thead>
            <tbody>
              {dataPresensis
                .filter((item) => {
                  if (searchTerm === "") {
                    return item;
                  } else if (
                    Object.values(item).some((val) =>
                      String(val)
                        .toLowerCase()
                        .includes(searchTerm.toLowerCase())
                    )
                  ) {
                    return item;
                  }
                })
                .map((item, index) => {
                  return (
                    <tr
                      key={index}
                      className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                    >
                      <td className="px-6 py-4">{item.tanggal}</td>
                      <td className="px-6 py-4">
                        <div className="text-base font-semibold">
                          {item.nama}
                        </div>
                      </td>
                      <td className="px-6 py-4">{item.jamHadir}</td>
                      <td className="px-6 py-4">{item.jamKeluar}</td>
                      <td className="px-6 py-4">
                        <img
                          className="w-10 h-10 rounded-full"
                          src={item.fotoHadir}
                          onError={(e) => {
                            e.target.onerror = null;
                            e.target.src = IMGNoPhoto;
                          }}
                        />
                      </td>
                      <td className="px-6 py-4">
                        <img
                          className="w-10 h-10 rounded-full"
                          src={item.fotoKeluar}
                          onError={(e) => {
                            e.target.onerror = null;
                            e.target.src = IMGNoPhoto;
                          }}
                        />
                      </td>
                    </tr>
                  );
                })}
            </tbody>
          </table>
        </div>
      </div>

      <Modal show={showModal} onClose={() => setShowModal(!showModal)}>
        <Modal.Header>Tambah Presensi Manual</Modal.Header>
        <form
          onSubmit={handleSubmit(btnAddPresensi)}
          className="flex flex-col gap-4"
        >
          <Modal.Body>
            <div className="space-y-6">
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="nama" value="Nama Pegawai" />
                </div>
                <Controller
                  control={control}
                  defaultValue={""}
                  name="idPegawais"
                  render={({ field, ref }) => (
                    <Select
                      menuPortalTarget={
                        typeof window !== "undefined" ? document.body : null
                      }
                      styles={{
                        menuPortal: (base) => ({ ...base, zIndex: 9999 }),
                      }}
                      inputRef={ref}
                      placeholder={"Pilih Pegawai"}
                      isMulti
                      classNamePrefix="addl-class"
                      options={dataPegawais}
                      value={getValues("idPegawais")}
                      onChange={(val) => {
                        field.onChange(val);
                      }}
                    />
                  )}
                />
                {errors.nip && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="tanggal" value="Tanggal" />
                </div>
                <TextInput
                  id="tanggal"
                  placeholder="Masukkan Tanggal"
                  required
                  type="date"
                  {...register("tanggal", { required: true })}
                />
                {errors.tanggal && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
            </div>
          </Modal.Body>
          <Modal.Footer>
            <Button color="gray" onClick={() => setShowModal(!showModal)}>
              Batalkan
            </Button>
            <Button type="submit">Simpan</Button>
          </Modal.Footer>
        </form>
      </Modal>

      <Modal
        show={showModalFilter}
        onClose={() => setShowModalFilter(!showModalFilter)}
      >
        <Modal.Header>Filter Presensi</Modal.Header>
        <form
          onSubmit={handleSubmitFilter(getPresensis)}
          className="flex flex-col gap-4"
        >
          <Modal.Body>
            <div className="space-y-6">
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="tanggalMulai" value="Tanggal Awal" />
                </div>
                <TextInput
                  id="tanggalMulai"
                  placeholder="Masukkan Tanggal Awal"
                  required
                  type="date"
                  {...registerFilter("tanggalAwal", { required: true })}
                />
                {errorsFilter.tanggalAwal && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="tanggalAkhir" value="Tanggal Akhir" />
                </div>
                <TextInput
                  id="tanggalAkhir"
                  placeholder="Masukkan Tanggal Akhir"
                  required
                  type="date"
                  {...registerFilter("tanggalAkhir", { required: true })}
                />
                {errorsFilter.tanggalAkhir && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
            </div>
          </Modal.Body>
          <Modal.Footer>
            <Button
              color="gray"
              onClick={() => setShowModalFilter(!showModalFilter)}
            >
              Batalkan
            </Button>
            <Button type="submit">Simpan</Button>
          </Modal.Footer>
        </form>
      </Modal>
    </Layout>
  );
};

export default Laporan;
