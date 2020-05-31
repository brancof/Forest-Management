import React from 'react';
import {
  BrowserRouter,
  Switch,
  Route,
  Link,
  Redirect
} from "react-router-dom";
import Login from './Login';
import Register from './Register';
import Proprietarios from './Proprietarios';
import Inspetores from './Inspetores';
import Trabalhadores from './Trabalhadores';
import Supervisores from './Supervisores';
import Navbar from './Navbar';

class AppRoutes extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: '',
            accounttype: 'proprietarios',
            user: ''
        };

        this.setUsername = this.setUsername.bind(this);
        this.setPassword = this.setPassword.bind(this);
        this.setType = this.setType.bind(this);
        this.setUser = this.setUser.bind(this);
    }

    setUsername(newusername)
    {
        this.setState({username: newusername});
    }

    setUser(newuser)
    {
        this.setState({user: newuser});
    }

    setPassword(newpass)
    {
        this.setState({password: newpass});
    }

    setType(newtype)
    {
        this.setState({accounttype: newtype});
    }


    render() {
        return (
          <div className="AppRouter">
            <Switch>          
              <Route path="/login">
                {this.state.username === '' ? <Login change={{username: this.setUsername, 
                                                            password: this.setPassword, 
                                                            accounttype: this.setType,
                                                            user: this.setUser}} /> 
                : <Redirect to="/" />}
              </Route>
              <Route path="/registo">
                <Register />
              </Route>

              <Route path="/proprietarios">
                <Navbar change={{username: this.setUsername, 
                            password: this.setPassword, 
                            accounttype: this.setType,
                            user: this.setUser}}/> 
                {!(this.state.username === '') ? <Proprietarios user={this.state.user} username={this.state.username} password={this.state.password} />
                : <Redirect to="/" />}
              </Route>

              <Route path="/inspetores">
                <Navbar change={{username: this.setUsername, 
                            password: this.setPassword, 
                            accounttype: this.setType,
                            user: this.setUser}}/> 
                {!(this.state.username === '') ? <Inspetores username={this.state.username} password={this.state.password} />
                : <Redirect to="/" />}
              </Route> 

              <Route path="/trabalhadores">
                <Navbar change={{username: this.setUsername, 
                            password: this.setPassword, 
                            accounttype: this.setType,
                            user: this.setUser}}/> 
                {!(this.state.username === '') ? <Trabalhadores user={this.state.user} username={this.state.username} password={this.state.password} />
                : <Redirect to="/" />}
              </Route> 

              <Route path="/supervisores">
                <Navbar change={{username: this.setUsername, 
                            password: this.setPassword, 
                            accounttype: this.setType,
                            user: this.setUser}}/> 
                {!(this.state.username === '') ? <Supervisores username={this.state.username} password={this.state.password} />
                : <Redirect to="/" />}
              </Route> 

              <Route exact path="/">
                {this.state.username === '' ? <Redirect to="/login"/> : <Redirect to={"/"+this.state.accounttype}/>}
              </Route>
            </Switch>
          </div>
        );
    }
}

export default AppRoutes;