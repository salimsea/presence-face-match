import { useForm } from "react-hook-form";
import { Layout } from "../components";
import { Card, Button, Label, TextInput } from "flowbite-react";
import { instanceAuth } from "../helpers";
import Swal from "sweetalert2";
import { useEffect } from "react";

const Pengaturan = () => {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm();

  useEffect(() => {
    getPengaturan();
  }, []);

  const getPengaturan = () => {
    instanceAuth.get("/presensi/getPengaturan").then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        setValue("hightlight", data.data.hightlight);
        setValue("waktuHadir", data.data.waktuHadir);
        setValue("waktuKeluar", data.data.waktuKeluar);
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };

  const btnSave = (data) => {
    var fd = new FormData();
    fd.append("hightlight", data.hightlight);
    fd.append("waktuHadir", data.waktuHadir);
    fd.append("waktuKeluar", data.waktuKeluar);
    instanceAuth.post("/presensi/setpengaturan", fd).then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        Swal.fire(
          "Berhasil",
          "Anda berhasil memperbarui pengaturan aplikasi!",
          "success"
        );
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };
  return (
    <Layout>
      <Card>
        <form onSubmit={handleSubmit(btnSave)}>
          <div className="grid grid-cols-2 gap-6 mb-8">
            <div className="col-span-2">
              <div>
                <div className="mb-2 block">
                  <Label htmlFor="highLight" value="Highlight Aplikasi" />
                </div>
                <TextInput
                  id="highLight"
                  placeholder="Masukkan highlight aplikasi android"
                  required
                  type="text"
                  {...register("hightlight", { required: true })}
                />
                {errors.hightlight && (
                  <span className="text-sm text-red-500">
                    This field is required
                  </span>
                )}
              </div>
            </div>
            <div>
              <div className="mb-2 block">
                <Label htmlFor="waktuHadir" value="Waktu Hadir" />
              </div>
              <TextInput
                id="waktuHadir"
                placeholder="Masukkan waktu hadir presensi"
                required
                type="time"
                {...register("waktuHadir", { required: true })}
              />
              {errors.waktuHadir && (
                <span className="text-sm text-red-500">
                  This field is required
                </span>
              )}
            </div>
            <div>
              <div className="mb-2 block">
                <Label htmlFor="waktuKeluar" value="Waktu Keluar" />
              </div>
              <TextInput
                id="waktuKeluar"
                placeholder="Masukkan waktu keluar presensi"
                required
                type="time"
                {...register("waktuKeluar", { required: true })}
              />
              {errors.waktuKeluar && (
                <span className="text-sm text-red-500">
                  This field is required
                </span>
              )}
            </div>
          </div>
          <Button type="submit">Simpan</Button>
        </form>
      </Card>
    </Layout>
  );
};

export default Pengaturan;
