import * as React from 'react';
import {NavigationContainer} from '@react-navigation/native';
import Routers from './routers';
import FaceSDK from '@regulaforensics/react-native-face-api';

function App() {
  FaceSDK.init(
    json => {
      var response = JSON.parse(json);
      if (!response['success']) {
        console.log('Init failed: ');
        console.log(json);
      }
    },
    e => {},
  );
  return (
    <NavigationContainer>
      <Routers />
    </NavigationContainer>
  );
}

export default App;
