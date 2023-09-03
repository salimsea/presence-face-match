import { useEffect, useState } from "react";
import { Layout } from "../components";
import {
  Button,
  Modal,
  FileInput,
  Label,
  TextInput,
  Select,
} from "flowbite-react";
import { instanceAuth } from "../helpers";
import Swal from "sweetalert2";
import { useForm } from "react-hook-form";
import { IMGNoPhoto } from "../assets";

const Pegawai = () => {
  const [isEdit, setIsEdit] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [dataPegawais, setDataPegawais] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    formState: { errors },
  } = useForm();

  useEffect(() => {
    getPegawais();
  }, []);

  const modalType = (val, id) => {
    setShowModal(!showModal);
    if (val === "edit") {
      setIsEdit(true);
      getPegawai(id);
    } else {
      setIsEdit(false);
      reset();
    }
  };

  const getPegawais = () => {
    Swal.showLoading();
    instanceAuth.get("/presensi/getpegawais").then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        Swal.close();
        setDataPegawais(data.data);
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  const getPegawai = (id) => {
    Swal.showLoading();
    instanceAuth.get(`/presensi/getpegawai?idpegawai=${id}`).then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        Swal.close();
        setValue("idPegawai", data.data.idPegawai);
        setValue("nama", data.data.nama);
        setValue("nip", data.data.nip);
        setValue("status", data.data.status);
        setValue("urlFile", data.data.urlFile);
        setValue("foto", data.data.urlFile);
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  const btnAdd = (data) => {
    Swal.showLoading();
    var fd = new FormData();
    fd.append("nama", data.nama);
    fd.append("nip", data.nip);
    fd.append("status", data.status);
    fd.append("foto", data.foto[0]);
    instanceAuth.post("/presensi/addpegawai", fd).then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        Swal.fire(
          "Berhasil",
          "Anda berhasil menambahkan pegawai baru!",
          "success"
        );
        setShowModal(false);
        getPegawais();
        reset();
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  const btnEdit = (data) => {
    Swal.showLoading();
    var fd = new FormData();
    fd.append("idPegawai", data.idPegawai);
    fd.append("nama", data.nama);
    fd.append("status", data.status);
    fd.append("nip", data.nip);
    if (data.foto) fd.append("foto", data.foto[0]);
    instanceAuth.post("/presensi/editpegawai", fd).then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        Swal.fire("Berhasil", "Anda berhasil memperbarui pegawai!", "success");
        setShowModal(false);
        getPegawais();
        reset();
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  // const btnDelete = (id, nip) => {
  //   Swal.fire({
  //     title: "Apakah anda yakin?",
  //     text: `Anda akan menghapus nip pegawai [${nip}]!`,
  //     icon: "warning",
  //     showCancelButton: true,
  //     confirmButtonColor: "#3085d6",
  //     cancelButtonColor: "#d33",
  //     confirmButtonText: "Ya, saya yakin!",
  //   }).then((result) => {
  //     if (result.isConfirmed) {
  //       Swal.showLoading();
  //       instanceAuth
  //         .delete(`/presensi/deletepegawai?idpegawai=${id}`)
  //         .then((res) => {
  //           let data = res.data;
  //           if (data.isSuccess) {
  //             Swal.fire(
  //               "Berhasil",
  //               `Anda berhasil mengapus data nip pegawai [${nip}]`,
  //               "success"
  //             ).then(function () {
  //               getPegawais();
  //             });
  //           } else {
  //             Swal.fire("Gagal", `${data.returnMessage}`, "error");
  //           }
  //         });
  //     }
  //   });
  // };
  return (
    <Layout>
      <div className="">
        <div className=" sm:rounded-lg">
          <div className="flex items-center justify-between pb-4  dark:bg-gray-900">
            <div>
              <Button onClick={() => modalType("add")}>Tambah Pegawai</Button>
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
                  Nama
                </th>
                <th scope="col" className="px-6 py-3">
                  NIP
                </th>
                <th scope="col" className="px-6 py-3">
                  Status Pegawai
                </th>
                <th scope="col" className="px-6 py-3">
                  Dibuat Oleh
                </th>
                <th scope="col" className="px-6 py-3">
                  #
                </th>
              </tr>
            </thead>
            <tbody>
              {dataPegawais
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
                      <th
                        scope="row"
                        className="flex items-center px-6 py-4  whitespace-nowrap dark:text-white"
                      >
                        <img
                          className="w-10 h-10 rounded-full"
                          src={item.urlFile}
                          alt={item.nama}
                          onError={(e) => {
                            e.target.onerror = null;
                            e.target.src = IMGNoPhoto;
                          }}
                        />
                        <div className="pl-3">
                          <div className="text-base font-semibold">
                            {item.nama}
                          </div>
                        </div>
                      </th>
                      <td className="px-6 py-4">{item.nip}</td>
                      <td className="px-6 py-4">
                        {item.status === 1 ? (
                          <div className="flex items-center">
                            <div className="h-2.5 w-2.5 rounded-full bg-green-500 mr-2" />
                            Aktif
                          </div>
                        ) : (
                          <div className="flex items-center">
                            <div className="h-2.5 w-2.5 rounded-full bg-red-500 mr-2" />
                            Tidak Aktif
                          </div>
                        )}
                      </td>
                      <td className="px-6 py-4">
                        <div className="flex items-center">
                          {item.createdBy}
                        </div>
                      </td>
                      <td className="px-6 py-4">
                        <a
                          href="#"
                          onClick={() => modalType("edit", item.idPegawai)}
                          className="font-medium text-blue-600 dark:text-blue-500 hover:underline"
                        >
                          Edit
                        </a>
                      </td>
                    </tr>
                  );
                })}
            </tbody>
          </table>
        </div>
      </div>

      <Modal show={showModal} onClose={() => setShowModal(!showModal)}>
        <Modal.Header>{isEdit ? "Perbarui Data" : "Tambah Data"}</Modal.Header>
        <form
          onSubmit={handleSubmit(isEdit ? btnEdit : btnAdd)}
          className="flex flex-col gap-4"
        >
          <Modal.Body>
            <div className="space-y-6">
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="nip" value="NIP" />
                </div>
                <TextInput
                  id="nip"
                  placeholder="Masukkan NIP"
                  required
                  type="number"
                  {...register("nip", {
                    required: true,
                    minLength: 5,
                    maxLength: 10,
                  })}
                />
                {errors.nip && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="nama" value="Nama" />
                </div>
                <TextInput
                  id="nama"
                  placeholder="Masukkan Nama Pegawai"
                  required
                  type="text"
                  {...register("nama", { required: true })}
                />
                {errors.nama && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="status" value="Status Pegawai" />
                </div>
                <Select
                  id="status"
                  required
                  {...register("status", { required: true })}
                >
                  <option value={1}>Aktif</option>
                  <option value={-1}>Tidak Aktif</option>
                </Select>
                {errors.status && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
              <div className="max-w-md" id="fileUpload">
                <div className="mb-2 block">
                  <Label htmlFor="file" value="Pas Foto" />
                </div>
                <FileInput
                  // helperText="Maksimal ukuran foto 5mb"
                  id="file"
                  {...register("foto", { required: !isEdit })}
                />
                {errors.foto && (
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
    </Layout>
  );
};

export default Pegawai;
