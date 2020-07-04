/* global google */
import React from "react";
import { Map, InfoWindow, Marker, GoogleApiWrapper } from "google-maps-react";

class DirectionsMap extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      origem:  { lat: this.props.Data[0].latitude, lng:this.props.Data[0].longitude},
      destino: {lat: this.props.Data[this.props.Data.length-1].latitude, lng: this.props.Data[this.props.Data.length-1].longitude}
    }
    this.handleMapReady = this.handleMapReady.bind(this);
  }
  
  handleMapReady(mapProps, map) {
    this.calculateAndDisplayRoute(map);
  }

  calculateAndDisplayRoute(map) {
    const directionsService = new google.maps.DirectionsService();
    const directionsDisplay = new google.maps.DirectionsRenderer();
    directionsDisplay.setMap(map);
     
    var aux = this.props.Data.slice();
    aux.pop();
    aux.shift();
    
    const waypoints = aux.map(item =>{
      return{
        location: {lat: item.latitude, lng:item.longitude},
        stopover: true
      }
    })
    const origin = this.state.origem;
    const destination = this.state.destino ;
    
    directionsService.route({
      origin: origin,
      destination: destination,
      waypoints: waypoints,
      travelMode: 'DRIVING',
    }, (response, status) => {
      if (status === 'OK') {
        //console.log(response);
        directionsDisplay.setDirections(response);
      } else {
        window.alert('Directions request failed due to ' + status);
      }
    });
  }

  render() {
    return (
      <div style={{height: '50%', width: '50%'}}>
        <Map
          google={this.props.google}
          className={"map"}
          zoom={10}
          initialCenter={{ lat:41.5618, lng:-8.29563 }}
          onReady={this.handleMapReady}
        >
        </Map>
        </div>
      
    );
  }
}

export default GoogleApiWrapper({
    apiKey: ('AIzaSyD94bNwC33Z03mXP2n1toNLXj8eCAQgOYQ'),
    libraries: ["visualization"]
})(DirectionsMap)