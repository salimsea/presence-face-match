import {Image, StyleSheet, Text, TouchableOpacity, View} from 'react-native';
import React, {useState} from 'react';
import {fontFamilys, fontSizes} from '../utilis';
import {IMGBgHome, IMGFace, IMGLoading} from '../images';
import FaceSDK, {
  FaceCaptureResponse,
} from '@regulaforensics/react-native-face-api';
import axios from 'axios';

const Home = ({navigation}) => {
  const [isLoading, setIsLoading] = useState(false);
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
            .post('http://pfm-api.s34l.my.id/api/Presence/Checkin', fd, {
              headers: {
                accept: 'application/json',
                'content-type': 'multipart/form-data',
              },
            })
            .then(ret => {
              var data = ret.data;
              setIsLoading(false);
              if (data.isSuccess) {
                navigation.navigate('Result', {
                  PARAMStatus: true,
                  PARAMData: data.data,
                });
              } else {
                navigation.navigate('Result', {
                  PARAMStatus: false,
                  PARAMData: null,
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
        }}>
        <View style={{width: 50, height: 100, backgroundColor: '#B76DFE'}} />
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
          Semoga harimu menyenangkan...
        </Text>
        <View style={{alignItems: 'flex-start', marginTop: 30}}>
          <Button onPress={btnLiveness} />
        </View>
      </View>

      {/* BUTTON */}

      {/* BG LAYER */}
      <View style={{position: 'absolute', bottom: 0}}>
        <Image source={IMGBgHome} resizeMode="cover" />
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
          backgroundColor: '#B76DFE',
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
        }}>
        sistem sedang mencari wajah anda...
      </Text>
    </View>
  );
};

export default Home;

const styles = StyleSheet.create({});
