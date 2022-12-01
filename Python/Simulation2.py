from mesa import Agent, Model 

from mesa.space import SingleGrid

from mesa.time import SimultaneousActivation

from mesa.datacollection import DataCollector

import matplotlib
import matplotlib.pyplot as plt
import matplotlib.animation as animation
plt.rcParams["animation.html"] = "jshtml"
matplotlib.rcParams['animation.embed_limit'] = 2**128

import numpy as np
import pandas as pd

import time
import datetime

import sys

from http.server import BaseHTTPRequestHandler, HTTPServer
import logging
import json

def get_grid(model):
    """
    Defines information collected by agents in the grid
    parameters: Model
    return: np.array
    """

    grid = np.zeros( (model.grid.width, model.grid.height) )
    for (content, x, y) in model.grid.coord_iter():
        if content == None:
            grid[x][y] = 0
        else:
            if content.car_malfunction == True:
                grid[x][y] = 200
            else:
                grid[x][y] = 100
    return grid

class CarAgent(Agent):
    """
    Class car of type Agent
    parameters: Agent
    """

    def __init__(self, unique_id, model, car_malfunction, num_cars_allowed):
        """
        Initializes the class 
        parameters: CarAgent, int, Model, bool, int
        """

        super().__init__(unique_id, model)
        self.velocity = 60
        self.acceleration = 0
        self.brake = 0
        self.sec_distance = 1
        self.intention = self.random.randint(0, 1) #0 left | 1 right
        self.show_intention = True
        self.car_malfunction = car_malfunction
        self.num_cars_allowed = num_cars_allowed
        self.counter = 0
        self.go_forward = False
        self.front_malfunction = False
                   
    def changeLane(self, neighbor):
        """
        Change the lane of center cars
        parameters: CarAgent, CarAgent
        """

        if neighbor != None:
            neighbor.velocity = 0
        self.show_intention = False
        x, y = self.pos
        self.velocity = 60
        if self.intention == 0:
            if self.model.grid[x - 1][y] == None:
                self.model.grid.move_agent(self, (x - 1, y))
                if neighbor != None:
                    neighbor.counter += 1
        elif self.intention == 1:
            if self.model.grid[x + 1][y] == None:
                self.model.grid.move_agent(self, (x + 1, y))
                if neighbor != None:
                    neighbor.counter += 1
            
    def getWaitingCar(self):
        """
        Gets the car on the back diagonal of the center depending on the car's intention
        parameters: CarAgent
        return: tuple
        """

        if self.intention == 0 and self.pos[1]:
            waiting_car_pos = (self.pos[0] - 1, self.pos[1] + 1)
        elif self.intention == 1 and self.pos[1]:
            waiting_car_pos = (self.pos[0] + 1, self.pos[1] + 1)
        return waiting_car_pos
    
    def getPassingCar(self):
        """
        Gets the car whose front car is the malfunction one
        parameters: CarAgent
        return: tuple
        """

        if self.pos[0] == 0:
            passing_car = (self.pos[0] + 1, self.pos[1] - 1)
        elif self.pos[0] == 2:
            passing_car = (self.pos[0] - 1, self.pos[1] - 1)
        
        return passing_car
    
    def canPass(self, neighbor):
        """
        Moves the right or left lane cars
        parameters: CarAgent, CarAgent
        """

        if neighbor.show_intention == True:
            intention = neighbor.intention
            carLane = self.pos[0]
            if (intention == 0 and carLane == 0) or (intention == 1 and carLane == 2):
                if self.counter >= self.num_cars_allowed:
                    self.velocity = 60
            elif (intention == 0 and carLane == 2) or (intention == 1 and carLane == 0):
                self.velocity = 60
                
    def step(self):
        """
        Defines the actions of the CarAgent
        parameters: CarAgent
        """

        neighbors = self.model.grid.get_neighbors(self.pos, moore=True, include_center=False)
                
        if self.car_malfunction == True and self.pos[1] == (self.model.grid.height / 2):
            self.velocity = 0
            self.model.accident_occured = True
            self.show_intention = False
        elif self.model.accident_occured == True and self.pos[1] <= (self.model.grid.height / 2) + 1:
            self.velocity = 60
        
        if self.pos[0] == 1 and self.pos[1] <= self.model.grid.height - 5 and self.car_malfunction == False and self.model.accident_occured == True:    
            waiting_car_pos = self.getWaitingCar()
            diagonal_car = self.model.grid[waiting_car_pos[0]][waiting_car_pos[1]]
            if diagonal_car != None:
                if diagonal_car.counter < diagonal_car.num_cars_allowed:
                    self.changeLane(diagonal_car)
            elif diagonal_car == None:
                self.changeLane(diagonal_car)
        elif self.pos[0] == 0 or self.pos[0] == 2 :
            passing_car = self.getPassingCar()
            front_passing_car = self.model.grid[passing_car[0]][passing_car[1]]
            if front_passing_car != None:
                self.canPass(front_passing_car)
            elif front_passing_car == None and (self.pos[0] == 0 or self.pos[0] == 2):
                self.velocity = 60
  
        if self.velocity == 60 and self.model.grid[self.pos[0]][self.pos[1] - 1] == None:
            self.model.grid.move_agent(self, (self.pos[0], self.pos[1] - 1))

def get_positions(model):
    """
    Gets the position and id of every CarAgent and creates
    an array of objects
    parameters: Model
    return: Json
    """

    positions = []
    for (content, x, y) in model.grid.coord_iter():
        if content != None:
            car_properties = {"id": content.unique_id, "posx": x, "posy": y, "car_malfunction": content.car_malfunction}
            positions.append(car_properties)
    return json.dumps(positions)

class RoadModel(Model):
    """
    Class RoadModel of type Model
    parameters: Model
    """

    def __init__(self, width, height):
        """
        Initializes the class
        parameters: Model, int, int
        """

        self.schedule = SimultaneousActivation(self)
        self.accident_occured = False
        self.grid = SingleGrid(width, height, False)
        self.id_counter = 0
        self.count_steps = 0
        self.middle_cars = 0
        self.right_cars = 0
        self.left_cars = 0
        
        self.datacollector = DataCollector(model_reporters={"Grid":get_grid})
        
    def step(self):
        """
        Defines the actions of the model
        parameters: Model
        """

        self.datacollector.collect(self)
        for (content, x, y) in self.grid.coord_iter():
            if content != None and y == 0:
                self.grid.remove_agent(content)
                self.schedule.remove(content)
        
        if self.count_steps == 150:
            accidented_car = CarAgent(self.id_counter, self, True, 0)
            x = 1
            y = self.grid.height - 1
            self.grid.place_agent(accidented_car,(x, y))
            self.schedule.add(accidented_car)
            self.id_counter += 1
        else:
            car_spawn = CarAgent(self.id_counter, self, False, self.random.randint(0, 2))
            x = self.random.randint(0, 2)
            if x == 0:
                self.left_cars += 1
            elif x == 1:
                self.middle_cars += 1
            elif x == 2:
                self.right_cars += 1

            y = self.grid.height - 1
            if self.grid[x][y] == None:
                self.grid.place_agent(car_spawn ,(x, y))
                self.schedule.add(car_spawn)
                self.id_counter += 1
        self.count_steps += 1
        self.schedule.step()

MAX_ITERATIONS = 300
WIDTH = 3
HEIGHT = 102

model = RoadModel(WIDTH, HEIGHT)
        
all_grid = model.datacollector.get_model_vars_dataframe()

class Server(BaseHTTPRequestHandler):
    """
    Class Server of type BaseHTTPRequestHandler
    parameters: BaseHTTPRequestHandler
    """

    
    def _set_response(self):
        """
        Sets the response headers
        parameters: BaseHTTPRequestHandler
        """

        self.send_response(200)
        self.send_header('Content-type', 'text/html')
        self.end_headers()
        
    def do_GET(self):
        """
        Sets the GET response
        parameters: BaseHTTPRequestHandler
        """

        logging.info("GET request,\nPath: %s\nHeaders:\n%s\n", str(self.path), str(self.headers))
        model.step()
        positions_step = get_positions(model)
        self._set_response()
        resp = "{\"data\":" + positions_step + "}"
        self.wfile.write(resp.encode('utf-8'))
        # self.wfile.write("GET request for {}".format(self.path).encode('utf-8'))

    def do_POST(self):
        """
        Sets the POST response
        parameters: BaseHTTPRequestHandler
        """

        content_length = int(self.headers['Content-Length'])
        #post_data = self.rfile.read(content_length)
        post_data = json.loads(self.rfile.read(content_length))
        #logging.info("POST request,\nPath: %s\nHeaders:\n%s\n\nBody:\n%s\n",
                     #str(self.path), str(self.headers), post_data.decode('utf-8'))
        logging.info("POST request,\nPath: %s\nHeaders:\n%s\n\nBody:\n%s\n",
                     str(self.path), str(self.headers), json.dumps(post_data))
        
        model.step()
        positions_step = get_positions(model)
        self._set_response()
        resp = "{\"data\":" + positions_step + "}"
        self.wfile.write(resp.encode('utf-8'))
        print("Left Cars:", model.left_cars)
        print("Middle Cars:", model.middle_cars)
        print("Right Cars:", model.right_cars)

def run(server_class=HTTPServer, handler_class=Server, port=8585):
    """
    Runs the server
    parameters: BaseHTTPRequestHandler
    """

    logging.basicConfig(level=logging.INFO)
    server_address = ('', port)
    httpd = server_class(server_address, handler_class)
    logging.info("Starting httpd...\n") # HTTPD is HTTP Daemon!
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:   # CTRL+C stops the server
        pass
    httpd.server_close()
    logging.info("Stopping httpd...\n")

if __name__ == '__main__':
    from sys import argv
    
    if len(argv) == 2:
        run(port=int(argv[1]))
    else:
        run()
