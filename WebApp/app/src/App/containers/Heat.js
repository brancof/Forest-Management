import React, { Component } from 'react';
import { Map, HeatMap, GoogleApiWrapper } from 'google-maps-react';





class Heat extends Component {
  constructor(props) {
    super(props);
    this.state = {
        zoom: 12,
        center: {
          lat:41.5618, lng:-8.29563
        },
    }
  };




    render() {
      return (
          <div style={{height: '50%', width: '75%'}}>
              <Map
                google={this.props.google}
                zoom={this.props.Latitude==null? 10: this.state.zoom}
                style={{height: '50%', width: '80%'}}
                initialCenter={this.props.Latitude==null? this.state.center : {lat:this.props.Latitude, lng:this.props.Longitude}}
              >
            <HeatMap
              positions={this.props.HeatData}
              radius= {20}
              opacity= {1}
              maxIntensity={5}
            />
            </Map>
          </div>
      );
    }
  }



export default GoogleApiWrapper({
    apiKey: ('AIzaSyD94bNwC33Z03mXP2n1toNLXj8eCAQgOYQ'),
    libraries: ["visualization"]
})(Heat)