import {Image, StyleSheet, Text, TouchableOpacity, View} from 'react-native';
import React, {useEffect} from 'react';
import {IMGSuccess, IMGFailed} from '../images';
import {fontFamilys, fontSizes} from '../utilis';

const Result = ({navigation, route}) => {
  const {PARAMStatus, PARAMData, PARAMMsg} = route.params;
  useEffect(() => {
    setTimeout(() => {
      navigation.goBack();
    }, 10000);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <View style={{flex: 1, backgroundColor: 'white'}}>
      {/* ICON */}
      <View style={{marginBottom: 30, paddingHorizontal: 20}}>
        <View style={{alignSelf: 'center', marginTop: 50, marginBottom: 20}}>
          <Image
            style={{width: 100, height: 100}}
            source={PARAMStatus ? IMGSuccess : IMGFailed}
            resizeMode="contain"
          />
        </View>
        <Text
          style={{
            fontSize: fontSizes.body,
            fontFamily: fontFamilys.primary[700],
            color: '#1D2125',
            textAlign: 'center',
          }}>
          {PARAMStatus ? 'Presensi Berhasil' : 'Presensi Gagal'}
        </Text>
        {!PARAMStatus && (
          <Text
            style={{
              fontSize: fontSizes.body,
              fontFamily: fontFamilys.primary[400],
              color: '#1D2125',
              textAlign: 'center',
              marginTop: 10,
            }}>
            {PARAMMsg}
          </Text>
        )}
      </View>

      {/* CARD */}
      {PARAMStatus && (
        <View style={{paddingHorizontal: 20}}>
          <View
            style={{
              borderWidth: 1,
              borderColor: '#2A3890',
              borderRadius: 10,
              padding: 20,
              flexDirection: 'row',
            }}>
            <View style={{marginRight: 15}}>
              <Image
                source={{uri: PARAMData.urlFile}}
                style={{width: 100, height: 105, borderRadius: 10}}
                resizeMode="cover"
              />
            </View>
            <View style={{flex: 1}}>
              <LabelValue label={'NIP'} value={PARAMData?.nip} />
              <LabelValue label={'Nama'} value={PARAMData?.nama} />
              <LabelValue
                label={'Jam Hadir'}
                value={PARAMData?.presensiHariIni?.jamHadir}
              />
              <LabelValue
                label={'Jam Keluar'}
                value={PARAMData?.presensiHariIni?.jamKeluar}
              />
            </View>
          </View>
        </View>
      )}

      {/* BUTTON */}
      <View style={{position: 'absolute', bottom: 20, width: '100%'}}>
        <View style={{alignSelf: 'center', marginBottom: 10}}>
          <Button onPress={() => navigation.goBack()} />
        </View>
        <Text
          style={{
            fontSize: fontSizes.small,
            fontFamily: fontFamilys.primary[400],
            color: '#B8B8B8',
            textAlign: 'center',
          }}>
          Kembali otomatis dalam
        </Text>
        <Text
          style={{
            fontSize: fontSizes.small,
            fontFamily: fontFamilys.primary[400],
            color: '#B8B8B8',
            textAlign: 'center',
          }}>
          10 detik
        </Text>
      </View>
    </View>
  );
};

const Button = ({onPress}) => {
  return (
    <TouchableOpacity onPress={onPress}>
      <View
        style={{
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
          }}>
          Kembali
        </Text>
      </View>
    </TouchableOpacity>
  );
};

const LabelValue = ({label, value}) => {
  return (
    <View>
      <Text
        style={{
          fontSize: fontSizes.small,
          fontFamily: fontFamilys.primary[700],
          color: '#1D2125',
        }}>
        {label} :
      </Text>
      <Text
        style={{
          fontSize: fontSizes.bodyParagraph,
          fontFamily: fontFamilys.primary[400],
          color: '#1D2125',
        }}>
        {value || '-'}
      </Text>
    </View>
  );
};

export default Result;

const styles = StyleSheet.create({});
