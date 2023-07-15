import {StyleSheet, Text, TouchableOpacity, View} from 'react-native';
import React from 'react';
import FaceSDK, {
  FaceCaptureResponse,
} from '@regulaforensics/react-native-face-api';
import axios from 'axios';

const Scan = () => {
  const btnLiveness = () => {
    FaceSDK.presentFaceCaptureActivity(
      result => {
        if (result.image !== null) {
          var bitmap = FaceCaptureResponse.fromJson(JSON.parse(result)).image
            .bitmap;
          var fd = new FormData();
          fd.append('bitmap_face', bitmap);
          axios
            .post('http://pfm-api.s34l.my.id/api/presensi/checkin', fd, {
              headers: {
                accept: 'application/json',
                'content-type': 'multipart/form-data',
              },
            })
            .then(ret => console.log(ret.data));
        }
      },
      e => {
        console.log('here', e);
      },
    );
  };
  return (
    <View>
      <Text>Scan</Text>
      <TouchableOpacity
        onPress={btnLiveness}
        style={{padding: 10, backgroundColor: 'red'}}>
        <Text>play</Text>
      </TouchableOpacity>
    </View>
  );
};

export default Scan;

const styles = StyleSheet.create({});
