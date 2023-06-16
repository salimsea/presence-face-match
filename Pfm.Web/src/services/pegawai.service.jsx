import { instance } from "../helpers";

class PegawaiService {
  getPegawais() {
    return instance.get(`presence/getpegawais`);
  }
  getPegawai(id) {
    return instance.get(`presence/getpegawai?idPegawai=${id}`);
  }
  addPegawai(data) {
    return instance.post(`presence/addpegawai`, data);
  }
  editPegawai(data) {
    return instance.post(`presence/editpegawai`, data);
  }
  deletePegawai(id) {
    return instance.delete(`presence/deletepegawai?idPegawai=${id}`);
  }
}

export default new PegawaiService();
