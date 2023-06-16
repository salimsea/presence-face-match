import React from 'react';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import {Home, Result, Scan} from '../pages';

const Stack = createNativeStackNavigator();

const Routers = () => {
  return (
    <Stack.Navigator
      initialRouteName="Home"
      screenOptions={{headerShown: false}}>
      <Stack.Screen name="Home" component={Home} />
      <Stack.Screen name="Scan" component={Scan} />
      <Stack.Screen name="Result" component={Result} />
    </Stack.Navigator>
  );
};

export default Routers;
