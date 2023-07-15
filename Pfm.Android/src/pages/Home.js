import {ActivityIndicator, Dimensions, Image, StyleSheet, Text, TouchableOpacity, View} from 'react-native';
import React, {useEffect, useState} from 'react';
import {fontFamilys, fontSizes} from '../utilis';
import {IMGBgHome, IMGFace, IMGLoading, IMGLogo} from '../images';
import FaceSDK, {
  FaceCaptureResponse,
} from '@regulaforensics/react-native-face-api';
import axios from 'axios';

const Home = ({navigation}) => {
  const [isLoading, setIsLoading] = useState(false);
  const [dataPengaturan, setDataPengaturan] = useState(null);
  useEffect(() => {
    getPengaturan();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const {width: wp} = Dimensions.get('screen');

  // let url = 'https://d9a5-101-128-117-167.ngrok-free.app';
  let url = 'http://pfm-api.s34l.my.id';

  const getPengaturan = () => {
    axios.get(`${url}/api/presensi/getpengaturan`).then(res => {
      var data = res.data;
      if (data.isSuccess) {
        setDataPengaturan(data.data);
      }
    });
  };

  const btnLiveness = () => {
    FaceSDK.presentFaceCaptureActivity(
      result => {
        setIsLoading(true);
        result = FaceCaptureResponse.fromJson(JSON.parse(result));
        if (result.image) {
          var bitmap = result.image.bitmap;
          var fd = new FormData();
          fd.append('bitmap_face', bitmap);
          axios
            .post(`${url}/api/presensi/checkin`, fd, {
              headers: {
                accept: 'application/json',
                'content-type': 'multipart/form-data',
              },
            })
            .then(ret => {
              var data = ret.data;
              console.log(data);
              setIsLoading(false);
              if (data.isSuccess) {
                navigation.navigate('Result', {
                  PARAMStatus: true,
                  PARAMData: data.data,
                  PARAMMsg: data.returnMessage,
                });
              } else {
                navigation.navigate('Result', {
                  PARAMStatus: false,
                  PARAMData: null,
                  PARAMMsg: data.returnMessage,
                });
              }
            });
        } else {
          setIsLoading(false);
        }
      },
      e => {
        console.log('here', e);
      },
    );
  };
  if (isLoading) return <LoadingScreen />;
  return (
    <View style={{flex: 1, backgroundColor: 'white'}}>
      {/* LOGO */}
      <View
        style={{
          alignSelf: 'flex-end',
          paddingHorizontal: 20,
          marginBottom: 30,
          paddingTop: 30,
        }}>
        <Image
          source={IMGLogo}
          resizeMode="contain"
          style={{width: 70, height: 70}}
        />
      </View>

      {/* TEXT */}
      <View style={{paddingHorizontal: 20, marginBottom: 30}}>
        <Text
          style={{
            fontSize: fontSizes.heading5,
            fontFamily: fontFamilys.primary[700],
            color: '#1D2125',
          }}>
          Hai 👋
        </Text>
        <Text
          style={{
            fontSize: fontSizes.heading5,
            fontFamily: fontFamilys.primary[400],
            color: '#1D2125',
          }}>
          {dataPengaturan?.hightlight}
        </Text>

        <View style={{alignItems: 'flex-start', marginVertical: 30}}>
          <Button onPress={btnLiveness} />
        </View>
        <Text
          style={{
            fontSize: fontSizes.body,
            fontFamily: fontFamilys.primary[600],
            color: '#1D2125',
          }}>
          Waktu Presensi Kantor
        </Text>
        <Text
          style={{
            fontSize: fontSizes.body,
            fontFamily: fontFamilys.primary[400],
            color: '#1D2125',
          }}>
          Jam Masuk : {dataPengaturan?.waktuHadir}
        </Text>
        <Text
          style={{
            fontSize: fontSizes.body,
            fontFamily: fontFamilys.primary[400],
            color: '#1D2125',
          }}>
          Jam Keluar : {dataPengaturan?.waktuKeluar}
        </Text>
      </View>

      {/* BUTTON */}

      {/* BG LAYER */}
      <View style={{position: 'absolute', bottom: 0}}>
        <Image
          source={IMGBgHome}
          style={{width: wp, height: 300}}
          resizeMode="stretch"
        />
      </View>
    </View>
  );
};

const Button = ({onPress}) => {
  return (
    <TouchableOpacity onPress={onPress}>
      <View
        style={{
          flexDirection: 'row',
          alignItems: 'center',
          backgroundColor: '#EC3B39',
          paddingHorizontal: 30,
          paddingVertical: 10,
          borderRadius: 30,
        }}>
        <Text
          style={{
            fontSize: fontSizes.bodyParagraph,
            fontFamily: fontFamilys.primary[600],
            color: 'white',
            marginRight: 10,
          }}>
          Mulai Presensi
        </Text>
        <Image
          style={{width: 20, height: 20}}
          source={IMGFace}
          resizeMode="contain"
        />
      </View>
    </TouchableOpacity>
  );
};

const LoadingScreen = () => {
  return (
    <View
      style={{
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: 'white',
      }}>
      <Image
        style={{width: 200, height: 186, marginBottom: 30}}
        source={IMGLoading}
        resizeMode="contain"
      />
      <Text
        style={{
          fontSize: fontSizes.body,
          fontFamily: fontFamilys.primary[600],
          color: '#1D2125',
          marginBottom: 5,
        }}>
        Tunggu Sebentar
      </Text>
      <Text
        style={{
          fontSize: fontSizes.bodyParagraph,
          fontFamily: fontFamilys.primary[400],
          color: '#1D2125',
          marginBottom:20
        }}>
        sistem sedang mencari wajah anda...
      </Text>
      <ActivityIndicator color="red" size={40} />
    </View>
  );
};

export default Home;

const styles = StyleSheet.create({});
