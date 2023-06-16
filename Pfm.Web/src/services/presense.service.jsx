import { instance } from "../helpers";

class PresenceService {
  checkin(data) {
    return instance.post(`presence/checkin`, data);
  }
}

export default new PresenceService();
