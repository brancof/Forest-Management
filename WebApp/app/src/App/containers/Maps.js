import React from 'react';
import { Map, InfoWindow, Marker, GoogleApiWrapper } from 'google-maps-react';


class Maps extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            showingInfoWindow: false,
            activeMarker: {},
            selectedPlace: {},
            query:'',
            zoom: 11,
            center: {
                lat:41.5618, lng:-8.29563
            },
            AllPlaces: [
                {
                    name : "morada1",
                    lat: 41.7589,
                    lng:-8.329851,
                },
            
                {
                    name : "morada2",
                    lat: 41.7690,
                    lng:-8.329952,
                }
            ]
        }

        this.onMapClicked = this.onMapClicked.bind(this);
        this.onMarkerClick = this.onMarkerClick.bind(this);
    };

    

    markers = []

    onMarkerClick = (props, marker, e) => {
        this.setState({
            selectedPlace: props,
            activeMarker: marker,
            showingInfoWindow: true,
            zoom: 16
            
        });
    }

    onMapClicked = (props) => {
        if (this.state.showingInfoWindow) {
            this.setState({
                showingInfoWindow: false,
                activeMarker: null
            })
        }
        this.setState({zoom: 11, lat:41.5618, lng:-8.29563})
    }


    render() {
        return ( 
            <div style={{height: '25%', width: '75%'}}
            >
            <Map google={this.props.google}
                initialCenter = {this.state.center}
                center = {{lat:this.state.center.lat, lng:this.state.center.lng}}
                zoom={this.state.zoom}
                onClick={this.onMapClicked}
                style={{height: '25%', width: '80%'}}
                >
                {this.state.AllPlaces.map((marker, i) =>
                    <Marker
                        ref={(e) => {if (e) this.markers[i] = e.marker}}
                        onClick={this.onMarkerClick}
                        title = {marker.name}
                        name={marker.name}
                        position = {{lat:marker.lat,lng:marker.lng}}
                    />
                )}
                <InfoWindow
                    onOpen={this.windowHasOpened}
                    onClose={this.windowHasClosed}
                    marker={this.state.activeMarker}
                    visible={this.state.showingInfoWindow}>
                    <div>
                        <h1>{this.state.selectedPlace.name}</h1>
                    </div>
                </InfoWindow>
            </Map>
            </div>
        );
    }

}

export default GoogleApiWrapper({
    apiKey: ('AIzaSyD94bNwC33Z03mXP2n1toNLXj8eCAQgOYQ')
})(Maps)